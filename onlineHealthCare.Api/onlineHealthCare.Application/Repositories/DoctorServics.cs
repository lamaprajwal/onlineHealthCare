using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using onlineHealthCare.Application.Dtos;
using onlineHealthCare.Database;
using onlineHealthCare.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace onlineHealthCare.Application.Repositories
{
    public class DoctorServics : IDoctorService
    {
        public string _doctorimagePath = "wwwroot/Doctor/images";
        public IDbContextFactory<onlineHealthCareDbContext> Context { get; set; }
        public DoctorServics(IDbContextFactory<onlineHealthCareDbContext> context)
        {
            Context = context;
        }
        public DoctorDto Delete(int id)
        {
            throw new NotImplementedException();
        }

        

        public DoctorDto? GetDetails(int id)
        {
            throw new NotImplementedException();
        }

        public async Task Insert(DoctorDto t)
        {
            using var customContext = Context.CreateDbContext();
            var doctor = new Doctor
            {
                FirstName = t.FirstName,
                LastName = t.LastName,
                speciality = t.speciality,
                DoctorImage =  await UploadImage(t.DoctorImage, t.FirstName)
            };
            customContext.Add(doctor);
            customContext.SaveChanges();
        }
              

        public void Update(DoctorDto entity)
        {
            throw new NotImplementedException();
        }
        public FileStream ImageStream(string image)
        {
            return new FileStream(Path.Combine(_doctorimagePath, image), FileMode.Open, FileAccess.Read);
        }
        public async Task<string> UploadImage(IFormFile? Img,string doctorName )
        {
            try
            {
                // Ensure doctor ID is provided
                if (string.IsNullOrEmpty(doctorName))
                    throw new ArgumentException("Doctor ID cannot be null or empty");

                // Build the save path, creating the directory if it doesn't exist
                var savePath = Path.Combine(_doctorimagePath, doctorName); // Creates a folder per doctor
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }

                // Generate a unique file name based on the current date and time
                var fileExtension = Path.GetExtension(Img.FileName);
                var fileName = $"doctor_{DateTime.Now:dd-MM-yyyy-HH-mm-ss}{fileExtension}";

                // Save the file to the specified path
                var filePath = Path.Combine(savePath, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Img.CopyToAsync(fileStream);
                }

                // Construct the image URL to be returned and stored in the database
                var imageUrl = $"/content/doctors/{doctorName}/{fileName}";

                // Update the doctor's profile image URL in the database
                //using (var customContext = Context.CreateDbContext())
                //{
                //    var doctor = customContext.Doctors.FirstOrDefault(d => d.ID == doctorId);
                //    if (doctor == null)
                //        throw new Exception("Doctor not found");

                //    doctor.DoctorImage = imageUrl;
                //    //customContext.SaveChanges();
                //}

                return imageUrl; // Return the URL for further use
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "Error";
            }

        }

         

        public async Task<IEnumerable<DoctorResponse>> SearchDoctorsAsync(string? name, string? specialty)
        {
            using var customContext = Context.CreateDbContext();
            var query = customContext.Doctors.AsQueryable();

            // Search by name if provided
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(d => d.FirstName.Contains(name) || d.LastName.Contains(name));
            }

            // Search by specialty if provided
            if (!string.IsNullOrEmpty(specialty))
            {
                query = query.Where(d => d.speciality.Contains(specialty));
            }
           
            var doctors= await query.ToListAsync();
            var doctorResponse = doctors.Select(d => new DoctorResponse
            {

                FirstName = d.FirstName,
                LastName = d.LastName,
                speciality = d.speciality,
                DoctorImage = d.DoctorImage
                // Map other properties as needed
            }); ;

            return doctorResponse;
        }

        public async Task<List<DoctorResponse>> GetAll()
        {
            using var customContext = Context.CreateDbContext();
            var doctors = await customContext.Doctors.ToListAsync();

            // Map doctors to List<DoctorResponse>
            var doctorResponses = doctors.Select(d => new DoctorResponse
            {

                FirstName = d.FirstName,
                LastName = d.LastName,
                speciality = d.speciality,
                DoctorImage = d.DoctorImage,
                // Map other properties as needed
            }).ToList();

            return doctorResponses;
        }

        Task<List<DoctorDto>> IRepository<DoctorDto>.GetAll()
        {
            throw new NotImplementedException();
        }
    }


}

       
        

       

