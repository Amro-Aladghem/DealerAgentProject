import { AppHeader } from '@/components/layout/AppHeader';
import { ChatLauncher } from '@/components/chat/ChatLauncher';
import { ChatPanel } from '@/components/chat/ChatPanel';
import { CartProvider } from '@/contexts/CartContext';
import { ChatProvider } from '@/contexts/ChatContext';
import { matchPath, RouterProvider, useRouter } from '@/libs/router';
import { CartView } from '@/views/CartView';
import { HomeView } from '@/views/HomeView';
import { OrdersView } from '@/views/OrdersView';
import { ProductView } from '@/views/ProductView';

function CurrentView() {
  const { path } = useRouter();

  const carMatch = matchPath('/cars/:id', path);
  if (carMatch) return <ProductView kind="car" id={carMatch.id} />;

  const accessoryMatch = matchPath('/accessories/:id', path);
  if (accessoryMatch) return <ProductView kind="accessory" id={accessoryMatch.id} />;

  switch (path) {
    case '/cart':
      return <CartView />;
    case '/orders':
      return <OrdersView />;
    default:
      return <HomeView />;
  }
}

function AppShell() {
  return (
    <div className="flex min-h-screen flex-col bg-canvas">
      <AppHeader />
      <main className="flex-1 pb-24">
        <CurrentView />
      </main>
      <ChatLauncher />
      <ChatPanel />
    </div>
  );
}

function App() {
  return (
    <ChatProvider>
      <CartProvider>
        <RouterProvider>
          <AppShell />
        </RouterProvider>
      </CartProvider>
    </ChatProvider>
  );
}

export default App;
