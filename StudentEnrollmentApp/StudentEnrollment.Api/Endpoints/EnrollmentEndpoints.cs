using Microsoft.EntityFrameworkCore;
using StudentEnrollment.Data;

namespace StudentEnrollment.Api.Endpoints;

public static class EnrollmentEndpoints
{
    public static void MapEnrollmentEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Enrollment").WithTags(nameof(Enrollment));

        group.MapGet("/", async (StudentEnrollmentDbContext db) =>
        {
            return await db.Enrollments.ToListAsync();
        })
        .WithName("GetAllEnrollments")
        .WithOpenApi()
        .Produces<List<Enrollment>>(StatusCodes.Status200OK);

        group.MapGet("/{id}", async (int id, StudentEnrollmentDbContext db) =>
        {
            return await db.Enrollments.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Enrollment model
                    ? Results.Ok(model)
                    : Results.NotFound();
        })
        .WithName("GetEnrollmentById")
        .WithOpenApi()
        .Produces<Enrollment>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapPut("/{id}", async (int id, Enrollment enrollment, StudentEnrollmentDbContext db) =>
        {
            var affected = await db.Enrollments
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.CourseId, enrollment.CourseId)
                    .SetProperty(m => m.StudentId, enrollment.StudentId)
                    .SetProperty(m => m.Id, enrollment.Id)
                    .SetProperty(m => m.CreatedDate, enrollment.CreatedDate)
                    .SetProperty(m => m.CreatedBy, enrollment.CreatedBy)
                    .SetProperty(m => m.ModifiedDate, enrollment.ModifiedDate)
                    .SetProperty(m => m.ModifiedBy, enrollment.ModifiedBy)
                    );
            return affected == 1 ? Results.Ok() : Results.NotFound();
        })
        .WithName("UpdateEnrollment")
        .WithOpenApi()
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status204NoContent);

        group.MapPost("/", async (Enrollment enrollment, StudentEnrollmentDbContext db) =>
        {
            db.Enrollments.Add(enrollment);
            await db.SaveChangesAsync();
            return Results.Created($"/api/Enrollment/{enrollment.Id}", enrollment);
        })
        .WithName("CreateEnrollment")
        .WithOpenApi()
        .Produces<Enrollment>(StatusCodes.Status201Created);

        group.MapDelete("/{id}", async (int id, StudentEnrollmentDbContext db) =>
        {
            var affected = await db.Enrollments
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? Results.Ok() : Results.NotFound();
        })
        .WithName("DeleteEnrollment")
        .WithOpenApi()
        .Produces<Enrollment>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}
