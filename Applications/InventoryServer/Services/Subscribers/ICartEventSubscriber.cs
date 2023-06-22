namespace InventoryServer.Services.Subscribers
{
    public interface ICartEventSubscriber
    {
        void SubscribeToCartUpdateStarted();
        void SubscribeToAddToCartStarted();
        void SubscribeToRemoveFromCartStarted();
    }

}
