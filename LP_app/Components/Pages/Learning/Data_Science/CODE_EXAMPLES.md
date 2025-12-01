# Code Examples & Exercises

## 1. Statistics & Data Analysis

### Example 1: Descriptive Statistics
```python
import statistics
import numpy as np

# Sample dataset
data = [23, 45, 67, 34, 89, 56, 78, 43, 91, 54]

# Calculate statistics
mean = statistics.mean(data)
median = statistics.median(data)
mode = statistics.mode(data) if len(set(data)) < len(data) else "No mode"
stdev = statistics.stdev(data)
variance = statistics.variance(data)

print(f"Mean: {mean:.2f}")
print(f"Median: {median}")
print(f"Mode: {mode}")
print(f"Standard Deviation: {stdev:.2f}")
print(f"Variance: {variance:.2f}")
print(f"Min: {min(data)}, Max: {max(data)}")
print(f"Range: {max(data) - min(data)}")
```

### Example 2: Probability Distributions
```python
import numpy as np
from scipy import stats

# Normal distribution
x = np.linspace(-4, 4, 100)
y = stats.norm.pdf(x, 0, 1)

# Binomial distribution
n, p = 10, 0.5
k = np.arange(0, n+1)
binom_prob = stats.binom.pmf(k, n, p)

print("Normal Distribution Sample:")
samples = np.random.normal(0, 1, 1000)
print(f"Mean: {np.mean(samples):.2f}, Std: {np.std(samples):.2f}")

print("\nBinomial Distribution Probabilities:")
for i, prob in enumerate(binom_prob):
    print(f"P(X={i}): {prob:.4f}")
```

### Example 3: Hypothesis Testing
```python
from scipy import stats

# Two independent samples
group1 = [23, 45, 67, 34, 89]
group2 = [34, 56, 78, 43, 91]

# T-test
t_stat, p_value = stats.ttest_ind(group1, group2)

print(f"T-statistic: {t_stat:.4f}")
print(f"P-value: {p_value:.4f}")

if p_value < 0.05:
    print("Reject null hypothesis - groups are significantly different")
else:
    print("Fail to reject null hypothesis - groups are similar")
```

---

## 2. Python Programming

### Example 4: List Comprehensions
```python
# Basic list comprehension
squares = [x**2 for x in range(10)]
print(f"Squares: {squares}")

# With condition
even_squares = [x**2 for x in range(10) if x % 2 == 0]
print(f"Even squares: {even_squares}")

# Nested comprehension
matrix = [[i*j for j in range(1, 4)] for i in range(1, 4)]
print("Matrix:")
for row in matrix:
    print(row)

# Dictionary comprehension
squares_dict = {x: x**2 for x in range(1, 6)}
print(f"Squares dict: {squares_dict}")
```

### Example 5: Functions & Decorators
```python
# Basic function
def calculate_statistics(data):
    """Calculate mean and standard deviation"""
    mean = sum(data) / len(data)
    variance = sum((x - mean)**2 for x in data) / len(data)
    std_dev = variance ** 0.5
    return mean, std_dev

result = calculate_statistics([1, 2, 3, 4, 5])
print(f"Mean: {result[0]}, Std Dev: {result[1]:.2f}")

# Decorator
def timer_decorator(func):
    import time
    def wrapper(*args, **kwargs):
        start = time.time()
        result = func(*args, **kwargs)
        end = time.time()
        print(f"Execution time: {end - start:.4f} seconds")
        return result
    return wrapper

@timer_decorator
def slow_function(n):
    return sum(i**2 for i in range(n))

slow_function(1000000)
```

### Example 6: Dictionary Operations
```python
# Dictionary creation and manipulation
student = {
    "name": "John",
    "age": 25,
    "courses": ["Python", "Data Science", "ML"],
    "grades": {"Python": 95, "Data Science": 88, "ML": 92}
}

# Access elements
print(f"Name: {student['name']}")
print(f"Courses: {', '.join(student['courses'])}")

# Add new key-value
student["gpa"] = 3.8

# Iterate
print("\nAll grades:")
for course, grade in student["grades"].items():
    print(f"{course}: {grade}")

# Dictionary methods
print(f"Keys: {list(student.keys())}")
print(f"Values: {list(student.values())}")
```

---

## 3. SQL Queries

### Example 7: Basic SELECT Queries
```sql
-- Select all columns
SELECT * FROM students;

-- Select specific columns with WHERE clause
SELECT name, email, age FROM students 
WHERE age > 20 AND status = 'active';

-- ORDER BY and LIMIT
SELECT name, grade FROM students 
ORDER BY grade DESC 
LIMIT 10;

-- DISTINCT values
SELECT DISTINCT department FROM employees;
```

### Example 8: JOINs
```sql
-- INNER JOIN
SELECT s.name, c.course_name, e.grade
FROM students s
INNER JOIN enrollments e ON s.student_id = e.student_id
INNER JOIN courses c ON e.course_id = c.course_id;

-- LEFT JOIN
SELECT d.department_name, COUNT(e.employee_id) as count
FROM departments d
LEFT JOIN employees e ON d.dept_id = e.dept_id
GROUP BY d.department_name;

-- Multiple JOINs
SELECT s.name, c.course_name, g.grade_value
FROM students s
JOIN enrollments e ON s.student_id = e.student_id
JOIN courses c ON e.course_id = c.course_id
JOIN grades g ON e.enrollment_id = g.enrollment_id;
```

### Example 9: Subqueries & CTEs
```sql
-- Subquery
SELECT name, salary FROM employees
WHERE salary > (SELECT AVG(salary) FROM employees);

-- Common Table Expression (CTE)
WITH top_earners AS (
    SELECT employee_id, salary
    FROM employees
    WHERE salary > 100000
)
SELECT e.name, te.salary
FROM top_earners te
JOIN employees e ON te.employee_id = e.employee_id;

-- Nested subquery
SELECT name FROM employees
WHERE dept_id IN (
    SELECT dept_id FROM departments 
    WHERE location = 'New York'
);
```

---

## 4. Data Science

### Example 10: Pandas DataFrame
```python
import pandas as pd
import numpy as np

# Create DataFrame
df = pd.DataFrame({
    'name': ['Alice', 'Bob', 'Charlie', 'David'],
    'age': [25, 30, 35, 28],
    'salary': [50000, 60000, 75000, 55000],
    'department': ['Sales', 'IT', 'Finance', 'IT']
})

# Display info
print(df.head())
print(df.info())
print(df.describe())

# Filter and select
high_salary = df[df['salary'] > 60000]
it_dept = df[df['department'] == 'IT']

# Group by
dept_avg = df.groupby('department')['salary'].mean()
print(dept_avg)

# Missing values
df['bonus'] = [5000, np.nan, 8000, 6000]
df.fillna(0, inplace=True)

# Sort and rank
df_sorted = df.sort_values('salary', ascending=False)
df['salary_rank'] = df['salary'].rank(ascending=False)
```

### Example 11: Data Visualization
```python
import matplotlib.pyplot as plt
import numpy as np

# Line plot
x = np.linspace(0, 10, 100)
y = np.sin(x)
plt.figure(figsize=(10, 6))
plt.plot(x, y, label='sin(x)')
plt.xlabel('X')
plt.ylabel('Y')
plt.title('Sine Wave')
plt.legend()
plt.grid(True)
plt.show()

# Histogram
data = np.random.normal(100, 15, 1000)
plt.hist(data, bins=30, edgecolor='black')
plt.xlabel('Values')
plt.ylabel('Frequency')
plt.title('Distribution')
plt.show()

# Scatter plot
x = np.random.randn(100)
y = 2*x + np.random.randn(100)
plt.scatter(x, y, alpha=0.5)
plt.xlabel('X')
plt.ylabel('Y')
plt.title('Scatter Plot')
plt.show()
```

---

## 5. Code Challenges

### Challenge 1: Data Cleaning
```python
# Problem: Clean and analyze the following dataset
import pandas as pd

data = {
    'name': ['Alice', 'Bob', None, 'David', 'Eve'],
    'age': [25, 'N/A', 35, 28, 32],
    'salary': [50000, 60000, 75000, None, 55000],
    'email': ['alice@example.com', 'bob@example', 'charlie@example.com', 'david@example.com', 'eve@example.com']
}

df = pd.DataFrame(data)

# Tasks:
# 1. Handle missing values
# 2. Fix data types
# 3. Validate emails
# 4. Create summary statistics
```

### Challenge 2: Statistical Analysis
```python
# Problem: Analyze the relationship between variables
import pandas as pd
from scipy import stats

# Create sample dataset
df = pd.DataFrame({
    'study_hours': [2, 3, 4, 5, 6, 7, 8, 9],
    'test_scores': [55, 60, 65, 70, 75, 85, 90, 95]
})

# Tasks:
# 1. Calculate correlation
# 2. Perform linear regression
# 3. Create visualization
# 4. Interpret results
```

### Challenge 3: SQL Analysis
```sql
-- Problem: Find top performers by department
-- Tables: employees, departments, performance_reviews

-- Tasks:
-- 1. Join necessary tables
-- 2. Calculate average performance rating by department
-- 3. Find top 3 employees in each department
-- 4. Create summary with rankings
```

---

## Practice Exercises

### Exercise 1: Work with Lists
Write a function that:
- Takes a list of numbers
- Removes duplicates
- Sorts in descending order
- Returns the top 5 elements

### Exercise 2: Data Analysis
Using Pandas:
- Load CSV file
- Check for missing values
- Calculate correlation matrix
- Create visualization
- Export results

### Exercise 3: Database Query
Write SQL query that:
- Joins 3+ tables
- Filters with WHERE clause
- Groups with aggregation
- Orders and limits results
- Uses subquery

---

## Resources

- **Python Official Documentation:** https://docs.python.org/3/
- **Pandas Documentation:** https://pandas.pydata.org/docs/
- **NumPy Documentation:** https://numpy.org/doc/
- **SQL Tutorial:** https://www.w3schools.com/sql/
- **Kaggle Datasets:** https://www.kaggle.com/datasets

---

## Next Steps

1. Run all code examples
2. Modify and experiment
3. Complete practice exercises
4. Submit challenge solutions
5. Share in forum
6. Earn coding badges
