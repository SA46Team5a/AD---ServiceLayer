﻿using ServiceLayer.DataAccess;
using System;
using System.Collections.Generic;

namespace ServiceLayer
{
    // Author: Divyashree
    public interface IDepartmentService
    {
        // Retrieve
        Authority getDelegatedAuthority(string dept);
        Authority getCurrentAuthority(string dept);
        DepartmentRepresentative getCurrentDepartmentRepresentative(string dept);
        CollectionPoint getCollectionPointOfEmployee(string emp);
        bool verifyPassCode(string passcode, string dep);
        Employee getEmployeeById(string emp);
        List<Employee> getEmployeesOfDepartment(string dep);
        List<Department> getDepartments();
        string getDepartmentID(string emp);
        Employee getEmployeeObject(String empName);
        CollectionPoint getCollectionPointOfDepartment(string depId);
        List<Employee> getEligibleDepartmentRepresentatives(string deptId);
        List<Employee> getEligibleDelegatedAuthority(string deptId);

        // Create
        void addAuthority(Employee emp, DateTime startdate, DateTime enddate);


        // Update
        void updateAuthority(Authority auth);
        void rescindAuthority(string empId);        
        void updateDepartmentRepresentative(int currentDeptRepId, string newRepEmpId);
        void updateCollectionPoint(string dep, int cp);
        string generateNewPasscode(string dep);
      

    }
}