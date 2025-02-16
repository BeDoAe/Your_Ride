using AutoMapper;



namespace Your_Ride.Helper


{
    public class IFormFileToStringConverter : IValueConverter<IFormFile, string>
    {
        public string Convert(IFormFile source, ResolutionContext context)
        {
            if (source == null) return null;

            // Ensure proper async handling for file saving.
            var fileTask = FileHelper.SaveFileAsync(source);
            Task.WaitAll(fileTask); // Wait for the async method.
            return fileTask.Result; // Get the resulting file path.
        }
    }
}