export interface ApiResponse<T> {
  data: T;
  success: boolean;
  message: string | null;
  error: string | null;
}

export interface GetEmployeeDto {
  id: number;
  firstName: string | null;
  lastName: string | null;
  salary: number;
  dateOfBirth: string;
  dependents: GetDependentDto[] | null;
}

export interface GetDependentDto {
  id: number;
  firstName: string | null;
  lastName: string | null;
  dateOfBirth: string;
  relationship: Relationship;
}

export type Relationship = 0 | 1 | 2 | 3;

export interface PaycheckDto {
  grossAmount: number;
  totalDeductions: number;
  netAmount: number;
  payPeriodType: number;
  payPeriodTypeFriendlyName: string | null;
  employeeId: number;
  deductionBreakdown: DeductionLineDto[] | null;
}

export interface DeductionLineDto {
  amount: number;
  name: string | null;
}

export interface CreateEmployeeDto {
  firstName: string;
  lastName: string;
  salary: number;
  dateOfBirth: string;
  dependents: {
    firstName: string;
    lastName: string;
    dateOfBirth: string;
    relationship: Relationship;
  }[];
}

export interface UpdateEmployeeDto extends CreateEmployeeDto {}
