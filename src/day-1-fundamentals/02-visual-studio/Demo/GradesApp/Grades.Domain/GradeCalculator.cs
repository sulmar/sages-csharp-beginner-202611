namespace Grades.Domain;

// Klasa
public class GradeCalculator
{
    // Metoda statyczna
    public static double CalculateGrade(double grade1, double grade2, double grade3) // Parametry metody
    {
        return (grade1 + grade2 + grade3) / 3; // Zwraca wartość
    }
}