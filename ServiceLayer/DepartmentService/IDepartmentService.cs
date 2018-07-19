using ServiceLayer.DataAccess;
using System;
using System.Collections.Generic;

namespace ServiceLayer
{
    // Author: Divyashree
    public interface IDepartmentService
    {
        // Retrieve
        Authority getCurrentAuthority(string dept);
        DepartmentRepresentative getCurrentDepartmentRepresentative(string dept);

        CollectionPoint getCollectionPointOfEmployee(string emp);

        bool verifyPassCode(string passcode, string dep);
        Employee getEmployeeById(string emp);
        string getDepartmentID(string emp);

        // Create
        void addAuthority(Employee emp, DateTime startdate, DateTime enddate);

        // Update
        void updateAuthority(Authority auth);
        void rescindAuthority(Authority auth);
        
        void updateDepartmentRepresentative(int currentDeptRepId, string newRepEmpId);
        void updateCollectionPoint(string dep, int cp);
        string generateNewPasscode(string dep);

    }
}