namespace radar_drone_App_User
{
     class Battery
    {
        public double consumption { get; set; }
        public int capacity { get; set; }

        public  Battery(double pconsumption, int pcapacity)
        {
            consumption = pconsumption;
            capacity = pcapacity;
        }




    }
}