import {
  createContext,
  useCallback,
  useContext,
  useEffect,
  useMemo,
  useState,
} from 'react';
import type { ReactNode } from 'react';
import { STORE_ID } from '@/libs/config';
import storeService from '@/services/storeService';
import type { CartDto, CartItemDto } from '@/types/cart';

interface CartContextValue {
  cart: CartDto | null;
  items: CartItemDto[];
  count: number;
  loading: boolean;
  reload: () => void;
}

const CartContext = createContext<CartContextValue | null>(null);

export function CartProvider({ children }: { children: ReactNode }) {
  const [cart, setCart] = useState<CartDto | null>(null);
  const [items, setItems] = useState<CartItemDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [nonce, setNonce] = useState(0);

  const reload = useCallback(() => {
    setLoading(true);
    setNonce((value) => value + 1);
  }, []);

  useEffect(() => {
    let active = true;

    const load = async () => {
      const cartResponse = await storeService.getCart(STORE_ID);

      if (!active) return;

      if (!cartResponse.isSuccess || !cartResponse.data.id) {
        setCart(null);
        setItems([]);
        setLoading(false);
        return;
      }

      setCart(cartResponse.data);

      const itemsResponse = await storeService.getCartItems(STORE_ID, cartResponse.data.id);

      if (!active) return;

      setItems(itemsResponse.isSuccess ? itemsResponse.data : []);
      setLoading(false);
    };

    load();

    return () => {
      active = false;
    };
  }, [nonce]);

  const value = useMemo(() => {
    const count = items.reduce((total, item) => total + item.quantity, 0);
    return { cart, items, count, loading, reload };
  }, [cart, items, loading, reload]);

  return <CartContext.Provider value={value}>{children}</CartContext.Provider>;
}

export function useCart(): CartContextValue {
  const ctx = useContext(CartContext);
  if (!ctx) throw new Error('useCart must be used within a CartProvider');
  return ctx;
}
