using Microsoft.EntityFrameworkCore;
using StudentEnrollment.Data.Contracts;

namespace StudentEnrollment.Data.Repositories;

public class CourseRepository : GenericRepository<Course>, ICourseRepository
{
    public CourseRepository(StudentEnrollmentDbContext db) : base(db)
    {

    }

    public async Task<Course> GetStudentList(int courseId)
    {
        var course = await _db.Courses
            .Include(i => i.Enrollments).ThenInclude(i => i.Student)
            .FirstOrDefaultAsync();

        return course;
    }
}