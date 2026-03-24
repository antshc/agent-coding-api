import type { CreateEmployeeDto, GetEmployeeDto, PaycheckDto, UpdateEmployeeDto } from "@/types/api";

let nextId = 100;

export function deleteMockEmployee(id: number): void {
  const index = mockEmployees.findIndex((e) => e.id === id);
  if (index === -1) throw new Error(`Employee ${id} not found`);
  mockEmployees.splice(index, 1);
  delete mockPaychecks[id];
}


export const mockEmployees: GetEmployeeDto[] = [
  {
    id: 1,
    firstName: "LeBron",
    lastName: "James",
    salary: 75420.99,
    dateOfBirth: "1984-12-30",
    dependents: [
      { id: 1, firstName: "Savannah", lastName: "James", dateOfBirth: "1986-08-27", relationship: 1 },
      { id: 2, firstName: "Bronny", lastName: "James", dateOfBirth: "2004-10-06", relationship: 3 },
      { id: 3, firstName: "Bryce", lastName: "James", dateOfBirth: "2007-06-14", relationship: 3 },
    ],
  },
  {
    id: 2,
    firstName: "Ja",
    lastName: "Morant",
    salary: 92365.22,
    dateOfBirth: "1999-08-10",
    dependents: [
      { id: 4, firstName: "Kaari", lastName: "Morant", dateOfBirth: "2019-08-07", relationship: 3 },
    ],
  },
  {
    id: 3,
    firstName: "Michael",
    lastName: "Jordan",
    salary: 143211.12,
    dateOfBirth: "1963-02-17",
    dependents: [
      { id: 5, firstName: "Yvette", lastName: "Prieto", dateOfBirth: "1979-11-26", relationship: 2 },
      { id: 6, firstName: "Jasmine", lastName: "Jordan", dateOfBirth: "1992-12-07", relationship: 3 },
    ],
  },
  {
    id: 4,
    firstName: "Diana",
    lastName: "Taurasi",
    salary: 68500.00,
    dateOfBirth: "1982-06-11",
    dependents: [],
  },
  {
    id: 5,
    firstName: "Sue",
    lastName: "Bird",
    salary: 81200.50,
    dateOfBirth: "1980-10-16",
    dependents: [
      { id: 7, firstName: "Megan", lastName: "Rapinoe", dateOfBirth: "1985-07-05", relationship: 2 },
    ],
  },
];

function computePaycheck(emp: GetEmployeeDto): PaycheckDto {
  const periodsPerYear = 26;
  const grossAmount = +(emp.salary / periodsPerYear).toFixed(2);

  const baseCost = 1000 / periodsPerYear;
  const depCount = emp.dependents?.length ?? 0;
  const depCost = depCount * (600 / periodsPerYear);

  const dob = new Date(emp.dateOfBirth);
  const today = new Date();
  let age = today.getFullYear() - dob.getFullYear();
  const m = today.getMonth() - dob.getMonth();
  if (m < 0 || (m === 0 && today.getDate() < dob.getDate())) age--;
  const seniorSurcharge = age > 50 ? (emp.salary * 0.02) / periodsPerYear : 0;

  const highSalarySurcharge = emp.salary > 80000 ? (emp.salary * 0.02) / periodsPerYear : 0;

  const deductions: { name: string; amount: number }[] = [
    { name: "Base Benefits", amount: +baseCost.toFixed(2) },
  ];
  if (depCount > 0) deductions.push({ name: `Dependents (${depCount})`, amount: +depCost.toFixed(2) });
  if (seniorSurcharge > 0) deductions.push({ name: "Senior Surcharge (>50)", amount: +seniorSurcharge.toFixed(2) });
  if (highSalarySurcharge > 0) deductions.push({ name: "High Salary Surcharge", amount: +highSalarySurcharge.toFixed(2) });

  const totalDeductions = +deductions.reduce((s, d) => s + d.amount, 0).toFixed(2);

  return {
    grossAmount,
    totalDeductions,
    netAmount: +(grossAmount - totalDeductions).toFixed(2),
    payPeriodType: 26,
    payPeriodTypeFriendlyName: "Bi-Weekly (26 periods/year)",
    employeeId: emp.id,
    deductionBreakdown: deductions,
  };
}

export const mockPaychecks: Record<number, PaycheckDto> = Object.fromEntries(
  mockEmployees.map((emp) => [emp.id, computePaycheck(emp)])
);

export function addMockEmployee(dto: CreateEmployeeDto): GetEmployeeDto {
  const id = nextId++;
  let depId = nextId * 100;
  const emp: GetEmployeeDto = {
    id,
    firstName: dto.firstName,
    lastName: dto.lastName,
    salary: dto.salary,
    dateOfBirth: dto.dateOfBirth,
    dependents: dto.dependents.map((d) => ({
      id: depId++,
      firstName: d.firstName,
      lastName: d.lastName,
      dateOfBirth: d.dateOfBirth,
      relationship: d.relationship,
    })),
  };
  mockEmployees.push(emp);
  mockPaychecks[emp.id] = computePaycheck(emp);
  return emp;
}

export function updateMockEmployee(id: number, dto: UpdateEmployeeDto): GetEmployeeDto {
  const index = mockEmployees.findIndex((e) => e.id === id);
  if (index === -1) throw new Error(`Employee ${id} not found`);
  let depId = id * 1000;
  const updated: GetEmployeeDto = {
    id,
    firstName: dto.firstName,
    lastName: dto.lastName,
    salary: dto.salary,
    dateOfBirth: dto.dateOfBirth,
    dependents: dto.dependents.map((d) => ({
      id: depId++,
      firstName: d.firstName,
      lastName: d.lastName,
      dateOfBirth: d.dateOfBirth,
      relationship: d.relationship,
    })),
  };
  mockEmployees[index] = updated;
  mockPaychecks[updated.id] = computePaycheck(updated);
  return updated;
}
