namespace DotNetTribes.Services
{
    public class ProductionRulesService: IRulesService
    {
        public int StartingGold = 500;
        
        public int CalculateTownhallPrice(int level)
        {
            return level * 200;
        }
        
        public int CalculateFarmPrice(int level)
        {
            return level * 100;
        }
        
        public int CalculateMinePrice(int level)
        {
            return level * 100;
        }
        
        public int CalculateAcademyPrice(int level)
        {
            if (level == 1)
            {
                return 150;
            }
            return level * 100;
        }
        
        public int CalculateTroopPrice(int level)
        {
            return level * 25;
        }
        
        public int CalculateTownhallHP(int level)
        {
            return level * 200;
        }
        
        public int CalculateFarmHP(int level)
        {
            return level * 100;
        }
        
        public int CalculateMineHP(int level)
        {
            return level * 100;
        }
        
        public int CalculateAcademyHP(int level)
        {
            return level * 150;
        }
        
        public int CalculateTroopHp(int level)
        {
            return level * 20;
        }
        
        public int CalculateTownhallBuildingTime(int level)
        {
            if (level == 1)
            {
                return 120;
            }

            return level * 60;
        }
        
        public int CalculateFarmBuildingTime(int level)
        {
            return level * 60;
        }
        
        public int CalculateMineBuildingTime(int level)
        {
            return level * 60;
        }
        
        public int CalculateAcademyBuildingTime(int level)
        {
            if (level == 1)
            {
                return 90;
            }

            return level * 60;
        }
        
        public int CalculateTroopBuildingTime(int level)
        {
            return level * 30;
        }
        
        
        
        
        
    }
}