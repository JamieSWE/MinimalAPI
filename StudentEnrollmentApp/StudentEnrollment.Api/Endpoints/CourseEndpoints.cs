using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudentEnrollment.Api.DTOs.Course;
using StudentEnrollment.Data;

namespace StudentEnrollment.Api.Endpoints;

public static class CourseEndpoints
{
    public static void MapCourseEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Course").WithTags(nameof(Course));

        group.MapGet("/", async (StudentEnrollmentDbContext db, IMapper mapper) =>
        {
            var courses = await db.Courses.ToListAsync();
            return mapper.Map<List<CourseDto>>(courses);
        })
        .WithName("GetAllCourses")
        .WithOpenApi()
        .Produces<List<CourseDto>>(StatusCodes.Status200OK);

        group.MapGet("/{id}", async (int id, StudentEnrollmentDbContext db, IMapper mapper) =>
        {
            return await db.Courses.FindAsync(id)
               is Course course
               ? Results.Ok(mapper.Map<CourseDto>(course))
               : Results.NotFound();
        })
        .WithName("GetCourseById")
        .WithOpenApi()
        .Produces<CourseDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapPut("/{id}", async (int id, CourseDto courseDto, StudentEnrollmentDbContext db, IMapper mapper) =>
        {
            var foundModel = await db.Courses.FindAsync(id);
            if (foundModel is null) return Results.NotFound();

            mapper.Map(courseDto, foundModel);
            //db.Update(courseDto);
            await db.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithName("UpdateCourse")
        .WithOpenApi()
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status204NoContent);

        group.MapPost("/", async (CreateCourseDto courseDto, StudentEnrollmentDbContext db, IMapper mapper) =>
        {
            var course = mapper.Map<Course>(courseDto);
            await db.AddAsync(course);
            await db.SaveChangesAsync();

            return Results.Created($"/courses/{course.Id}", course);
        })
        .WithName("CreateCourse")
        .WithOpenApi()
        .Produces<Course>(StatusCodes.Status201Created);

        group.MapDelete("/{id}", async (int id, StudentEnrollmentDbContext db) =>
        {
            if (await db.Courses.FindAsync(id) is Course course)
            {
                db.Courses.Remove(course);
                await db.SaveChangesAsync();
                return Results.Ok(course);
            }
            return Results.NotFound();
        })
        .WithName("DeleteCourse")
        .WithOpenApi()
        .Produces<Course>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}
