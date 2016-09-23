using System;

namespace TaskQueueAndCancellationPractise.DB
{
    public class ClassExamedEntity 
    {
        public ClassExamedEntity(Guid id, Guid examId, Guid classId)
        {
            Id = id;
            ExamId = examId;
            ClassId = classId;
        }

        private ClassExamedEntity()
        {
        }

        public Guid Id { get; private set; }
        public Guid ExamId { get; private set; }
        public Guid ClassId { get; private set; }
    }
}
