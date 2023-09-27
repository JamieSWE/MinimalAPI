using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudentEnrollment.Api.DTOs.Student;
using StudentEnrollment.Data;

namespace StudentEnrollment.Api.Endpoints;

public static class StudentEndpoints
{
    public static void MapStudentEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Student").WithTags(nameof(Student));

        group.MapGet("/", async (StudentEnrollmentDbContext db, IMapper mapper) =>
        {
            var students = await db.Students.ToListAsync();
            return mapper.Map<List<StudentDto>>(students);
        })
        .WithName("GetAllStudents")
        .WithOpenApi()
        .Produces<List<StudentDto>>(StatusCodes.Status200OK);

        group.MapGet("/{id}", async (int id, StudentEnrollmentDbContext db, IMapper mapper) =>
        {
            return await db.Students.FindAsync(id)
                is Student student
                ? Results.Ok(mapper.Map<StudentDto>(student))
                : Results.NotFound();
        })
        .WithName("GetStudentById")
        .WithOpenApi()
        .Produces<StudentDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapPut("/{id}", async (int id, Student studentDto, StudentEnrollmentDbContext db, IMapper mapper) =>
        {
            var foundModel = await db.Students.FindAsync(id);
            if (foundModel is null) return Results.NotFound();

            mapper.Map(studentDto, foundModel);
            //db.Update(courseDto);
            await db.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithName("UpdateStudent")
        .WithOpenApi()
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status204NoContent);

        group.MapPost("/", async (CreateStudentDto studentDto, StudentEnrollmentDbContext db, IMapper mapper) =>
        {
            var student = mapper.Map<Student>(studentDto);
            await db.AddAsync(student);
            await db.SaveChangesAsync();

            return Results.Created($"/api/Student/{student.Id}", student);
        })
        .WithName("CreateStudent")
        .WithOpenApi()
        .Produces<Student>(StatusCodes.Status201Created);

        group.MapDelete("/{id}", async (int id, StudentEnrollmentDbContext db) =>
        {
            if (await db.Students.FindAsync(id) is Student student)
            {
                db.Students.Remove(student);
                await db.SaveChangesAsync();
                return Results.Ok(student);
            }
            return Results.NotFound();
        })
        .WithName("DeleteStudent")
        .WithOpenApi()
        .Produces<Student>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}
