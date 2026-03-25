import type { CreateEmployeeDto, GetEmployeeDto, PaycheckDto, UpdateEmployeeDto } from "@/types/api";
import { mockEmployees, mockPaychecks, addMockEmployee, updateMockEmployee, deleteMockEmployee } from "./mock-data";

const MOCK_DELAY = 400;

function delay<T>(value: T): Promise<T> {
  return new Promise((resolve) => setTimeout(() => resolve(value), MOCK_DELAY));
}

export function fetchEmployeesMock(): Promise<GetEmployeeDto[]> {
  return delay([...mockEmployees]);
}

export function fetchEmployeeMock(id: number): Promise<GetEmployeeDto> {
  const emp = mockEmployees.find((e) => e.id === id);
  if (!emp) return Promise.reject(new Error(`Employee ${id} not found`));
  return delay({ ...emp });
}

export function fetchPaycheckMock(employeeId: number): Promise<PaycheckDto> {
  const pc = mockPaychecks[employeeId];
  if (!pc) return Promise.reject(new Error(`Paycheck for employee ${employeeId} not found`));
  return delay({ ...pc });
}

export function createEmployeeMock(dto: CreateEmployeeDto): Promise<GetEmployeeDto> {
  const emp = addMockEmployee(dto);
  return delay(emp);
}

export function updateEmployeeMock(id: number, dto: UpdateEmployeeDto): Promise<GetEmployeeDto> {
  const emp = updateMockEmployee(id, dto);
  return delay(emp);
}

export function deleteEmployeeMock(id: number): Promise<void> {
  deleteMockEmployee(id);
  return delay(undefined);
}
