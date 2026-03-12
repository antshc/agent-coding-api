import { expect, test } from '@playwright/test';

const mockForecasts = [
  { date: '2026-03-12', temperatureC: 10, temperatureF: 50, summary: 'Chilly' },
  { date: '2026-03-13', temperatureC: 20, temperatureF: 68, summary: 'Warm' },
];

test.describe('Weather Forecasts', () => {
  test.beforeEach(async ({ page }) => {
    await page.route('**/api/v1/weather-forecasts', (route) =>
      route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify(mockForecasts),
      })
    );
  });

  test('displays mocked forecast rows', async ({ page }) => {
    await page.goto('/');

    const rows = page.locator('tbody tr');
    await expect(rows).toHaveCount(mockForecasts.length);

    await expect(rows.nth(0)).toContainText('2026-03-12');
    await expect(rows.nth(0)).toContainText('10');
    await expect(rows.nth(0)).toContainText('50');
    await expect(rows.nth(0)).toContainText('Chilly');

    await expect(rows.nth(1)).toContainText('2026-03-13');
    await expect(rows.nth(1)).toContainText('20');
    await expect(rows.nth(1)).toContainText('68');
    await expect(rows.nth(1)).toContainText('Warm');
  });

  test('shows table header columns', async ({ page }) => {
    await page.goto('/');

    await expect(page.locator('thead')).toContainText('Date');
    await expect(page.locator('thead')).toContainText('Temp. (C)');
    await expect(page.locator('thead')).toContainText('Temp. (F)');
    await expect(page.locator('thead')).toContainText('Summary');
  });
});
