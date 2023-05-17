namespace iotServer.NewsLetter{

    public enum Events
    {
        sensorUpdate,
    }


    public static class NewsLetter
    {
        //sensorUpdate
        public static event EventHandler<EventArgs> sensorUpdate;

        public static void OnSensorUpdate(EventArgs e)
        {
            sensorUpdate?.Invoke(null, e);
        }



    }

}