public class ConfigWrapper
{
    public string ConfigName { get; set; } // JSON string representing the configuration
    public string Description { get; set; } // Description of the configuration
    public string ColorTone { get; set; } // Color tone for the configuration
    public IFormFile LogoImage { get; set; } // Optional files associated with the configuration
    public IFormFile BackgroundImage { get; set; } // Required files for the configuration labels
}