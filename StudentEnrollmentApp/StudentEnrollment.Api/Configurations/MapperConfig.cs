using AutoMapper;
using StudentEnrollment.Api.DTOs.Course;
using StudentEnrollment.Data;

namespace StudentEnrollment.Api.Configurations;

public class MapperConfig : Profile
{
    public MapperConfig()
    {
        CreateMap<Course, CourseDto>().ReverseMap();
        CreateMap<Course, CreateCourseDto>().ReverseMap();
    }
}
