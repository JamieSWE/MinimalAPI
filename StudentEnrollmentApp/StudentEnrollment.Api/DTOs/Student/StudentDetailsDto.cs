using StudentEnrollment.Api.DTOs.Enrollment;

namespace StudentEnrollment.Api.DTOs.Student;

public class StudentDetailsDto : CreateStudentDto
{
    public List<EnrollmentDto> Enrollments { get; set; } = new List<EnrollmentDto>();
}
