using Application.Dtos;
using Application.Services;
using Domain.Enums;
using Infrastructure.ExternalServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Channels;

namespace Presentation
{
    [Route("api/v1/stores")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly ChatService _chatService;
        private readonly AgentDecisionService _agentDecisionService;
        private readonly NotificationService _notificationService;
        private readonly CarService _carService;
        private readonly CarAccessoryService _carAccessoryService;
        private readonly OrderService _orderService;
        private readonly OrderItemService _orderItemService;
        private readonly CartService _cartService;
        private readonly CartItemService _cartItemService;
        private readonly Channel<AgentRequestDto> _channel;

        public StoreController(ChatService chatService, AgentDecisionService agentDecisionService,
            NotificationService notificationService, CarService carService, CarAccessoryService carAccessoryService,
            OrderService orderService, OrderItemService orderItemService,
            CartService cartService, CartItemService cartItemService, Channel<AgentRequestDto> channel)
        {
            _chatService = chatService;
            _agentDecisionService = agentDecisionService;
            _notificationService = notificationService;
            _carService = carService;
            _carAccessoryService = carAccessoryService;
            _orderService = orderService;
            _orderItemService = orderItemService;
            _cartService = cartService;
            _cartItemService = cartItemService;
            _channel = channel;
        }

        [HttpPost("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ChatDto>> CreateChatForStore(Guid id, ChatDto chatDto)
        {
            var result = await _chatService.CreateChat(id, chatDto);

            return Ok(result);
        }

        [HttpPost("{storeId}/chats/{chatSignuture}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> ChatWithAgent(AgentRequestDto agentRequestDto,
            Guid storeId, Guid chatSignuture, [FromQuery] bool askUser = false)
        {
            string? agentType = null;

            if (!askUser && agentRequestDto.AgentType is not null)
            {
                agentType = await _agentDecisionService.GetAgentTypeAsync(agentRequestDto.Message);
                if (agentType is null)
                    return StatusCode(StatusCodes.Status500InternalServerError, "System Error , please try again");
            }

            await _chatService.CreateChat(storeId, new ChatDto()
            {
                ChatSignature = chatSignuture,
                StoreId = storeId,
                ChatRoleId = (int)eChatRole.User,
                Message = agentRequestDto.Message,
                ImageUrl = agentRequestDto.ImageUri,
                AgentType = agentType ?? agentRequestDto.AgentType,
                ShowMessage = true,
                CreationTime = DateTime.UtcNow
            });

            await _notificationService.CreateNotification(storeId, new NotificationDto()
            {
                StoreId = storeId,
                ChatId = chatSignuture,
                AgentType = agentType ?? agentRequestDto.AgentType!,
            });

            agentRequestDto.AgentType = agentType ?? agentRequestDto.AgentType!;
            agentRequestDto.StoreId = storeId;
            agentRequestDto.ChatSignture = chatSignuture;

            await _channel.Writer.WriteAsync(agentRequestDto);
            return Accepted(agentType);
        }

        [HttpGet("{storeId}/chats/{chatSignuture}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ChatDto>>> GetChatHistory(Guid storeId, Guid chatSignuture)
        {
            var result = await _chatService.GetChatsForChatId(storeId, chatSignuture);

            return Ok(result);
        }

        [HttpGet("{storeId}/chats/{chatSignuture}/notifications")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<NotificationDto>>> GetNotificationsForChat(Guid storeId, Guid chatSignuture)
        {
            var result = await _notificationService.GetNotificationsForChatId(storeId, chatSignuture);

            return Ok(result);
        }

        [HttpGet("{storeId}/cars")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<CarDto>>> GetCars(Guid storeId)
        {
            var result = await _carService.GetCars(storeId);

            return Ok(result);
        }

        [HttpGet("{storeId}/cars/{carId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CarDto>> GetCarDetails(Guid storeId, Guid carId)
        {
            var result = await _carService.GetCarById(storeId, carId);

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("{storeId}/accessories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<CarAccessoryDto>>> GetCarsAccessories(Guid storeId)
        {
            var result = await _carAccessoryService.GetCarAccessories(storeId);

            return Ok(result);
        }

        [HttpGet("{storeId}/accessories/{accessoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CarAccessoryDto>> GetAccessoryDetails(Guid storeId, Guid accessoryId)
        {
            var result = await _carAccessoryService.GetCarAccessoryById(storeId, accessoryId);

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("{storeId}/orders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<OrderDto>>> GetOrders(Guid storeId)
        {
            var result = await _orderService.GetOrdersForStore(storeId);

            return Ok(result);
        }

        [HttpGet("{storeId}/orders/{orderId}/items")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<OrderItemDto>>> GetOrderItemsForOrder(Guid storeId, Guid orderId)
        {
            var result = await _orderItemService.GetOrderItemsForOrder(orderId, storeId);

            return Ok(result);
        }

        [HttpGet("{storeId}/cart")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CartDto>> GetCart(Guid storeId)
        {
            var result = await _cartService.GetCartForStore(storeId);

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("{storeId}/cart/{cartId}/items")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<CartItemDto>>> GetCartItems(Guid storeId, Guid cartId)
        {
            var result = await _cartItemService.GetCartItemsForCart(cartId, storeId);

            return Ok(result);
        }
    }
}
