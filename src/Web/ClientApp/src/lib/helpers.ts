import type { Relationship } from "@/types/api";

const relationshipLabels: Record<Relationship, string> = {
  0: "Unknown",
  1: "Spouse",
  2: "Domestic Partner",
  3: "Child",
};

export function getRelationshipLabel(r: Relationship): string {
  return relationshipLabels[r] ?? "Unknown";
}

export function formatCurrency(amount: number): string {
  return new Intl.NumberFormat("en-US", {
    style: "currency",
    currency: "USD",
  }).format(amount);
}

export function formatDate(iso: string): string {
  if (!iso) return "—";

  const date = new Date(iso);
  if (Number.isNaN(date.getTime())) return "—";

  return date.toLocaleDateString("en-US", {
    year: "numeric",
    month: "short",
    day: "numeric",
  });
}

export function calculateAge(dob: string): number {
  if (!dob) return 0;

  const birth = new Date(dob);
  if (Number.isNaN(birth.getTime())) return 0;

  const today = new Date();
  let age = today.getFullYear() - birth.getFullYear();
  const m = today.getMonth() - birth.getMonth();
  if (m < 0 || (m === 0 && today.getDate() < birth.getDate())) age--;
  return age;
}

export function fullName(first: string | null, last: string | null): string {
  return [first, last].filter(Boolean).join(" ") || "—";
}
