namespace Your_Ride.Helper
{
    public class FileHelper
    {
       
            #region Old Save
            //public static async Task<string> SaveFileAsync(IFormFile file)
            //{
            //    if (file == null || file.Length == 0)
            //    {
            //        return null;
            //    }

            //    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");
            //    if (!Directory.Exists(uploadsFolder))
            //    {
            //        Directory.CreateDirectory(uploadsFolder);
            //    }

            //    var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            //    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            //    using (var fileStream = new FileStream(filePath, FileMode.Create))
            //    {
            //        await file.CopyToAsync(fileStream);
            //    }

            //    return "/Files/" + uniqueFileName;
            //} 
            #endregion

            public static async Task<string> SaveFileAsync(IFormFile file)
            {
                if (file == null || file.Length == 0)
                {
                    return null;
                }

                // Validate file type (e.g., images only)
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(extension))
                {
                    throw new InvalidOperationException("Invalid file type.");
                }

                // Limit file size (e.g., 5 MB)
                const long maxFileSize = 5 * 1024 * 1024; // 5 MB
                if (file.Length > maxFileSize)
                {
                    throw new InvalidOperationException("File size exceeds the limit.");
                }

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                return "/Files/" + uniqueFileName;
            }

        }
    }
