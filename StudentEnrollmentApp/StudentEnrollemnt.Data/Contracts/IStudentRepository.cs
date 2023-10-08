namespace StudentEnrollment.Data.Contracts;

public interface IStudentRepository : IGenericRepository<Student>
{
    Task<Student> GetStudentDetails(int studentId);
}
