public class ConfigWrapper
{
    public string ConfigName { get; set; } // JSON string representing the configuration
    public string Description { get; set; } // Description of the configuration
    public string ColorTone { get; set; } // Color tone for the configuration
    public IFormFile LogoImage { get; set; } // Optional files associated with the configuration
    public IFormFile BackgroundImage { get; set; } // Required files for the configuration labels
}

public class StreamConfigWrapper
{
    public string JsonData { get; set; } = null!; // Text color for the stream configuration
    public IFormFile Gif { get; set; } // Optional GIF image for the stream configuration
    public IFormFile SoundEffect { get; set; } // Optional sound effect for the stream configuration
}