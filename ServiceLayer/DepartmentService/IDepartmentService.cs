using ServiceLayer.DataAccess;
using System;
using System.Collections.Generic;

namespace ServiceLayer
{
    public interface IDepartmentService
    {
        // Retrieve
        List<CollectionPoint> getCollectionPoints();
        string getAuthority(string dept);
        List<Employee> getDepartmentRepresentative(string dept);

        CollectionPoint getCollectionPointOfDepartment(string emp);
        bool verifyPassCode(int passcode);
        Employee getEmployeeById(string emp);
        string getDepartmentID(string emp);

        // Create
        void addAuthority(Employee emp, DateTime startdate, DateTime enddate);

        // Update
        void updateAuthority(Authority auth);
        void rescindAuthority(Authority auth);
        void updateDepartmentRepresentative(Employee oldemp, Employee newemp);
        void updateCollectionPoint(Department dep, CollectionPoint cp);
        void generateNewPasscode(Department dep);

    }
}