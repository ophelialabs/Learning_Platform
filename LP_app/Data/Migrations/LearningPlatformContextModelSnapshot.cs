using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using LP_app.Models;

#nullable disable

namespace LP_app.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "10.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("LP_app.Models.Achievement", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<string>("Criteria")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Description")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Icon")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("nvarchar(100)");

                b.HasKey("Id");

                b.ToTable("Achievements");
            });

            modelBuilder.Entity("LP_app.Models.Certificate", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<int>("CourseId")
                    .HasColumnType("int");

                b.Property<DateTime>("IssuedDate")
                    .HasColumnType("datetime2");

                b.Property<int>("UserId")
                    .HasColumnType("int");

                b.Property<DateTime?>("ValidUntil")
                    .HasColumnType("datetime2");

                b.Property<string>("VerificationCode")
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("nvarchar(100)");

                b.HasKey("Id");

                b.HasIndex("CourseId");

                b.HasIndex("UserId", "CourseId")
                    .IsUnique();

                b.HasIndex("VerificationCode")
                    .IsUnique();

                b.ToTable("Certificates");
            });

            modelBuilder.Entity("LP_app.Models.Course", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<string>("Category")
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("nvarchar(100)");

                b.Property<DateTime>("CreatedAt")
                    .HasColumnType("datetime2");

                b.Property<string>("Description")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Duration")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("ImageUrl")
                    .HasColumnType("nvarchar(max)");

                b.Property<int>("LessonCount")
                    .HasColumnType("int");

                b.Property<string>("Level")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)");

                b.Property<string>("Title")
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnType("nvarchar(255)");

                b.HasKey("Id");

                b.ToTable("Courses");
            });

            modelBuilder.Entity("LP_app.Models.Enrollment", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<decimal>("CompletionPercentage")
                    .HasColumnType("decimal(18,2)");

                b.Property<DateTime?>("CompletedDate")
                    .HasColumnType("datetime2");

                b.Property<int>("CourseId")
                    .HasColumnType("int");

                b.Property<DateTime>("EnrolledDate")
                    .HasColumnType("datetime2");

                b.Property<string>("Status")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)");

                b.Property<int>("UserId")
                    .HasColumnType("int");

                b.HasKey("Id");

                b.HasIndex("CourseId");

                b.HasIndex("UserId", "CourseId")
                    .IsUnique();

                b.ToTable("Enrollments");
            });

            modelBuilder.Entity("LP_app.Models.Lesson", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<string>("Content")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<int>("CourseId")
                    .HasColumnType("int");

                b.Property<DateTime>("CreatedAt")
                    .HasColumnType("datetime2");

                b.Property<string>("Description")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<int>("Duration")
                    .HasColumnType("int");

                b.Property<int>("LessonOrder")
                    .HasColumnType("int");

                b.Property<string>("Title")
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnType("nvarchar(255)");

                b.Property<string>("VideoUrl")
                    .HasColumnType("nvarchar(max)");

                b.HasKey("Id");

                b.HasIndex("CourseId");

                b.ToTable("Lessons");
            });

            modelBuilder.Entity("LP_app.Models.LessonProgress", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<DateTime?>("CompletedDate")
                    .HasColumnType("datetime2");

                b.Property<bool>("IsCompleted")
                    .HasColumnType("bit");

                b.Property<int>("LessonId")
                    .HasColumnType("int");

                b.Property<int>("UserId")
                    .HasColumnType("int");

                b.Property<int>("WatchedDuration")
                    .HasColumnType("int");

                b.HasKey("Id");

                b.HasIndex("LessonId");

                b.HasIndex("UserId", "LessonId")
                    .IsUnique();

                b.ToTable("LessonProgress");
            });

            modelBuilder.Entity("LP_app.Models.Quiz", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<DateTime>("CreatedAt")
                    .HasColumnType("datetime2");

                b.Property<string>("Description")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<int>("LessonId")
                    .HasColumnType("int");

                b.Property<decimal>("PassingScore")
                    .HasColumnType("decimal(18,2)");

                b.Property<string>("Title")
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnType("nvarchar(255)");

                b.Property<int>("TimeLimit")
                    .HasColumnType("int");

                b.HasKey("Id");

                b.HasIndex("LessonId");

                b.ToTable("Quizzes");
            });

            modelBuilder.Entity("LP_app.Models.QuizAnswer", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<string>("Answer")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<int>("AnswerOrder")
                    .HasColumnType("int");

                b.Property<bool>("IsCorrect")
                    .HasColumnType("bit");

                b.Property<int>("QuestionId")
                    .HasColumnType("int");

                b.HasKey("Id");

                b.HasIndex("QuestionId");

                b.ToTable("QuizAnswers");
            });

            modelBuilder.Entity("LP_app.Models.QuizAttempt", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<DateTime>("AttemptDate")
                    .HasColumnType("datetime2");

                b.Property<int>("QuizId")
                    .HasColumnType("int");

                b.Property<decimal>("Score")
                    .HasColumnType("decimal(18,2)");

                b.Property<int>("TimeSpent")
                    .HasColumnType("int");

                b.Property<int>("UserId")
                    .HasColumnType("int");

                b.HasKey("Id");

                b.HasIndex("QuizId");

                b.HasIndex("UserId");

                b.ToTable("QuizAttempts");
            });

            modelBuilder.Entity("LP_app.Models.QuizQuestion", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<string>("Question")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<int>("QuestionOrder")
                    .HasColumnType("int");

                b.Property<string>("QuestionType")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)");

                b.Property<int>("QuizId")
                    .HasColumnType("int");

                b.HasKey("Id");

                b.HasIndex("QuizId");

                b.ToTable("QuizQuestions");
            });

            modelBuilder.Entity("LP_app.Models.User", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<string>("Country")
                    .HasMaxLength(100)
                    .HasColumnType("nvarchar(100)");

                b.Property<DateTime>("CreatedAt")
                    .HasColumnType("datetime2");

                b.Property<DateTime?>("DateOfBirth")
                    .HasColumnType("datetime2");

                b.Property<string>("Email")
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnType("nvarchar(255)");

                b.Property<string>("FirstName")
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("nvarchar(100)");

                b.Property<bool>("IsVerified")
                    .HasColumnType("bit");

                b.Property<string>("LastName")
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("nvarchar(100)");

                b.Property<string>("PasswordHash")
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnType("nvarchar(255)");

                b.Property<string>("PhoneNumber")
                    .HasMaxLength(20)
                    .HasColumnType("nvarchar(20)");

                b.Property<DateTime?>("UpdatedAt")
                    .HasColumnType("datetime2");

                b.HasKey("Id");

                b.HasIndex("Email")
                    .IsUnique();

                b.ToTable("Users");
            });

            modelBuilder.Entity("LP_app.Models.UserAchievement", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<int>("AchievementId")
                    .HasColumnType("int");

                b.Property<DateTime>("EarnedDate")
                    .HasColumnType("datetime2");

                b.Property<int>("UserId")
                    .HasColumnType("int");

                b.HasKey("Id");

                b.HasIndex("AchievementId");

                b.HasIndex("UserId", "AchievementId")
                    .IsUnique();

                b.ToTable("UserAchievements");
            });

            modelBuilder.Entity("LP_app.Models.Certificate", b =>
            {
                b.HasOne("LP_app.Models.Course", "Course")
                    .WithMany("Certificates")
                    .HasForeignKey("CourseId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("LP_app.Models.User", "User")
                    .WithMany("Certificates")
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Course");
                b.Navigation("User");
            });

            modelBuilder.Entity("LP_app.Models.Enrollment", b =>
            {
                b.HasOne("LP_app.Models.Course", "Course")
                    .WithMany("Enrollments")
                    .HasForeignKey("CourseId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("LP_app.Models.User", "User")
                    .WithMany("Enrollments")
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Course");
                b.Navigation("User");
            });

            modelBuilder.Entity("LP_app.Models.Lesson", b =>
            {
                b.HasOne("LP_app.Models.Course", "Course")
                    .WithMany("Lessons")
                    .HasForeignKey("CourseId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Course");
            });

            modelBuilder.Entity("LP_app.Models.LessonProgress", b =>
            {
                b.HasOne("LP_app.Models.Lesson", "Lesson")
                    .WithMany("Progress")
                    .HasForeignKey("LessonId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("LP_app.Models.User", "User")
                    .WithMany("LessonProgress")
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Lesson");
                b.Navigation("User");
            });

            modelBuilder.Entity("LP_app.Models.Quiz", b =>
            {
                b.HasOne("LP_app.Models.Lesson", "Lesson")
                    .WithMany("Quizzes")
                    .HasForeignKey("LessonId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Lesson");
            });

            modelBuilder.Entity("LP_app.Models.QuizAnswer", b =>
            {
                b.HasOne("LP_app.Models.QuizQuestion", "Question")
                    .WithMany("Answers")
                    .HasForeignKey("QuestionId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Question");
            });

            modelBuilder.Entity("LP_app.Models.QuizAttempt", b =>
            {
                b.HasOne("LP_app.Models.Quiz", "Quiz")
                    .WithMany("Attempts")
                    .HasForeignKey("QuizId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("LP_app.Models.User", "User")
                    .WithMany("QuizAttempts")
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Quiz");
                b.Navigation("User");
            });

            modelBuilder.Entity("LP_app.Models.QuizQuestion", b =>
            {
                b.HasOne("LP_app.Models.Quiz", "Quiz")
                    .WithMany("Questions")
                    .HasForeignKey("QuizId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Quiz");
            });

            modelBuilder.Entity("LP_app.Models.UserAchievement", b =>
            {
                b.HasOne("LP_app.Models.Achievement", "Achievement")
                    .WithMany("UserAchievements")
                    .HasForeignKey("AchievementId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("LP_app.Models.User", "User")
                    .WithMany("UserAchievements")
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Achievement");
                b.Navigation("User");
            });

            modelBuilder.Entity("LP_app.Models.Achievement", b =>
            {
                b.Navigation("UserAchievements");
            });

            modelBuilder.Entity("LP_app.Models.Course", b =>
            {
                b.Navigation("Certificates");
                b.Navigation("Enrollments");
                b.Navigation("Lessons");
            });

            modelBuilder.Entity("LP_app.Models.Lesson", b =>
            {
                b.Navigation("Progress");
                b.Navigation("Quizzes");
            });

            modelBuilder.Entity("LP_app.Models.Quiz", b =>
            {
                b.Navigation("Attempts");
                b.Navigation("Questions");
            });

            modelBuilder.Entity("LP_app.Models.QuizQuestion", b =>
            {
                b.Navigation("Answers");
            });

            modelBuilder.Entity("LP_app.Models.User", b =>
            {
                b.Navigation("Certificates");
                b.Navigation("Enrollments");
                b.Navigation("LessonProgress");
                b.Navigation("QuizAttempts");
                b.Navigation("UserAchievements");
            });

#pragma warning restore 612, 618
        }
    }
}
