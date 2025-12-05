using LP_app.Data;
using LP_app.Models;

namespace LP_app.Services;

public class SeedDataService
{
    public static async Task SeedAsync(LearningPlatformContext context)
    {
        // Only seed if database is empty
        if (context.Courses.Any())
            return;

        // Sample courses based on your UI
        var courses = new List<Course>
        {
            // Enterprise Courses
            new Course
            {
                Title = "Enterprise Architecture",
                Description = "Learn enterprise system design patterns, microservices, and scalability principles",
                Category = "Enterprise",
                Duration = "6 weeks",
                LessonCount = 12,
                Level = "Advanced",
                CreatedAt = DateTime.UtcNow
            },
            new Course
            {
                Title = "Cloud & DevOps",
                Description = "Master cloud platforms, containerization, and continuous deployment",
                Category = "Enterprise",
                Duration = "8 weeks",
                LessonCount = 15,
                Level = "Advanced",
                CreatedAt = DateTime.UtcNow
            },
            new Course
            {
                Title = "Geofencing Systems",
                Description = "Implement location-based services and geofence management systems",
                Category = "Enterprise",
                Duration = "4 weeks",
                LessonCount = 8,
                Level = "Advanced",
                CreatedAt = DateTime.UtcNow
            },

            // Learning Courses
            new Course
            {
                Title = "Data Science Fundamentals",
                Description = "Introduction to data analysis, machine learning, and statistical methods",
                Category = "Learning",
                Duration = "10 weeks",
                LessonCount = 20,
                Level = "Intermediate",
                CreatedAt = DateTime.UtcNow
            },
            new Course
            {
                Title = "Engineering Principles",
                Description = "Core engineering concepts and problem-solving methodologies",
                Category = "Learning",
                Duration = "8 weeks",
                LessonCount = 16,
                Level = "Beginner",
                CreatedAt = DateTime.UtcNow
            },
            new Course
            {
                Title = "Mathematics for Technology",
                Description = "Essential mathematics concepts for software and systems engineering",
                Category = "Learning",
                Duration = "12 weeks",
                LessonCount = 24,
                Level = "Intermediate",
                CreatedAt = DateTime.UtcNow
            },
            new Course
            {
                Title = "Physics & Technology",
                Description = "Apply physics principles to technology and systems design",
                Category = "Learning",
                Duration = "10 weeks",
                LessonCount = 18,
                Level = "Intermediate",
                CreatedAt = DateTime.UtcNow
            },
            new Course
            {
                Title = "Chemistry Essentials",
                Description = "Chemical principles and their applications in technology",
                Category = "Learning",
                Duration = "8 weeks",
                LessonCount = 14,
                Level = "Beginner",
                CreatedAt = DateTime.UtcNow
            },
            new Course
            {
                Title = "BioTechnology Program",
                Description = "Introduction to biotechnology and biomedical engineering",
                Category = "Learning",
                Duration = "12 weeks",
                LessonCount = 20,
                Level = "Intermediate",
                CreatedAt = DateTime.UtcNow
            },
            new Course
            {
                Title = "Aerospace Engineering",
                Description = "Principles of aircraft and aerospace system design",
                Category = "Learning",
                Duration = "14 weeks",
                LessonCount = 28,
                Level = "Advanced",
                CreatedAt = DateTime.UtcNow
            },

            // Research Courses
            new Course
            {
                Title = "Research Methodologies",
                Description = "Scientific research methods and academic paper writing",
                Category = "Research",
                Duration = "6 weeks",
                LessonCount = 12,
                Level = "Intermediate",
                CreatedAt = DateTime.UtcNow
            },
            new Course
            {
                Title = "Research Journals & Publication",
                Description = "Navigate academic publishing and peer-reviewed journals",
                Category = "Research",
                Duration = "4 weeks",
                LessonCount = 8,
                Level = "Intermediate",
                CreatedAt = DateTime.UtcNow
            }
        };

        await context.Courses.AddRangeAsync(courses);

        // Add some achievements
        var achievements = new List<Achievement>
        {
            new Achievement
            {
                Name = "First Course",
                Description = "Complete your first course",
                Criteria = "Complete 1 course"
            },
            new Achievement
            {
                Name = "Quiz Master",
                Description = "Complete 5 quizzes with perfect scores",
                Criteria = "Score 100% on 5 quizzes"
            },
            new Achievement
            {
                Name = "Dedication",
                Description = "Maintain a 7-day learning streak",
                Criteria = "Learn every day for 7 days"
            },
            new Achievement
            {
                Name = "Scholar",
                Description = "Complete 5 courses",
                Criteria = "Complete 5 courses"
            },
            new Achievement
            {
                Name = "Lifelong Learner",
                Description = "Complete 10 courses",
                Criteria = "Complete 10 courses"
            }
        };

        await context.Achievements.AddRangeAsync(achievements);
        await context.SaveChangesAsync();
    }
}
