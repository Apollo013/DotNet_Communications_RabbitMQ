namespace Models.Common.Constants
{
    public static class ConnectionConstants
    {
        /*=================================================================================
         * App Setting Names / Keys   
         * ===============================================================================*/

        /*
        --------
        USAGE
        --------

        <appSettings>
            <add key="co.port" value = 5762 />
            <add key="ex.name" value = "mycomany.direct.exchange" />
            <add key="q.durable" value = true />
        </appSettings>

        */

        // CONNECTION CONTSTANTS
        public const string CONNECTION_PORT = "co.port";
        public const string CONNECTION_HOST = "co.hostname";
        public const string CONNECTION_VHOST = "co.virtualhost";
        public const string CONNECTION_USERNAME = "co.username";
        public const string CONNECTION_PASSWORD = "co.password";
        public const string CONNECTION_URI = "co.uri";

        // EXCHANGE CONTSTANTS   
        public const string EXCHANGE_NAMEKEY = "ex.name";
        public const string EXCHANGE_TYPEKEY = "ex.type";
        public const string EXCHANGE_DURABILITYKEY = "ex.durable";
        public const string EXCHANGE_AUTODELETEKEY = "ex.autodelete";

        // QUEUE CONTSTANTS
        public const string QUEUE_NAMEKEY = "q.name";
        public const string QUEUE_ROUTINGKEY = "q.key";
        public const string QUEUE_EXCLUSIVEKEY = "q.exclusive";
        public const string QUEUE_DURABILITYKEY = "q.durable";
        public const string QUEUE_AUTODELETEKEY = "q.autodelete";

        // QUALITY OF SERVICE CONTSTANTS
        public const string QOS_PREFETCHSIZEKEY = "qos.prefetchsize";
        public const string QOS_PREFETCHCOUNTKEY = "qos.prefetchcount";
        public const string QOS_GLOBALKEY = "qos.global";



        /*=================================================================================
         * MISCELLANEOUS   
         * ===============================================================================*/
        public const string CONTENTTYPE_PLAINTEXT = "text/plain";
    }
}
