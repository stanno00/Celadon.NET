namespace DotNetTribes.Service
{
    public class KingdomService : IKingdomService
    {
        private readonly ApplicationContext ApplicationContext;

        public KingdomService(ApplicationContext applicationContext)
        {
            ApplicationContext = applicationContext;
        }
    }
}