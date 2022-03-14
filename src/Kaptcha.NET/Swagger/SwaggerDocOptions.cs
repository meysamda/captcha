namespace KaptchaNET.Swagger
{
    public class SwaggerDocOptions
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public SwaggerDocContactOptions Contact { get; set; }

        public class SwaggerDocContactOptions
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Url { get; set; }
        }
    }
}