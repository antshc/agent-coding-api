import type { ApiResponse, CreateEmployeeDto, GetEmployeeDto, PaycheckDto, UpdateEmployeeDto } from "@/types/api";
import { fetchEmployeesMock, fetchEmployeeMock, fetchPaycheckMock, createEmployeeMock, updateEmployeeMock, deleteEmployeeMock } from "./mock-api";

/**
 * Set to true to use mock data instead of hitting the real API.
 * Toggle via VITE_USE_MOCK_API env var or hardcode here.
 */
const USE_MOCK = false;

type UserApiDto = {
  id: number;
  firstName: string | null;
  lastName: string | null;
};

const BASE_URL = (import.meta.env.VITE_API_BASE_URL ?? "").replace(/\/$/, "");

function toRequestUrl(path: string): string {
  return BASE_URL ? `${BASE_URL}${path}` : path;
}

function mapUserToEmployee(user: UserApiDto): GetEmployeeDto {
  return {
    id: user.id,
    firstName: user.firstName,
    lastName: user.lastName,
    salary: 0,
    dateOfBirth: "",
    dependents: [],
  };
}

async function request<T>(path: string, options?: RequestInit): Promise<T> {
  const res = await fetch(toRequestUrl(path), options);
  if (!res.ok) throw new Error(`API error: ${res.status} ${res.statusText}`);
  if (res.status === 204) return undefined as T;

  const contentLength = res.headers.get("content-length");
  if (contentLength === "0") return undefined as T;

  const body: ApiResponse<T> = await res.json();
  if (!body.success) throw new Error(body.error ?? body.message ?? "Request failed");
  return body.data;
}

export function fetchEmployees(): Promise<GetEmployeeDto[]> {
  if (USE_MOCK) return fetchEmployeesMock();
  return request<UserApiDto[]>("/Users").then((users) => users.map(mapUserToEmployee));
}

export function fetchEmployee(id: number): Promise<GetEmployeeDto> {
  if (USE_MOCK) return fetchEmployeeMock(id);
  return request<UserApiDto>(`/Users/${id}`).then(mapUserToEmployee);
}

export function fetchPaycheck(employeeId: number): Promise<PaycheckDto> {
  if (USE_MOCK) return fetchPaycheckMock(employeeId);
  return request<PaycheckDto>(`/api/v1/employees/${employeeId}/paychecks?periodtype=26`).catch((error: Error) => {
    if (error.message.includes("404")) {
      return {
        grossAmount: 0,
        totalDeductions: 0,
        netAmount: 0,
        payPeriodType: 26,
        payPeriodTypeFriendlyName: "Paycheck API not available",
        employeeId,
        deductionBreakdown: [],
      };
    }

    throw error;
  });
}

export function createEmployee(dto: CreateEmployeeDto): Promise<GetEmployeeDto> {
  if (USE_MOCK) return createEmployeeMock(dto);
  return request<GetEmployeeDto>("/api/v1/Employees", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(dto),
  });
}

export function updateEmployee(id: number, dto: UpdateEmployeeDto): Promise<GetEmployeeDto> {
  if (USE_MOCK) return updateEmployeeMock(id, dto);
  return request<GetEmployeeDto>(`/api/v1/Employees/${id}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(dto),
  });
}

export function deleteEmployee(id: number): Promise<void> {
  if (USE_MOCK) return deleteEmployeeMock(id);
  return request<void>(`/api/v1/Employees/${id}`, { method: "DELETE" });
}
