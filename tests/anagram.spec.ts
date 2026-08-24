import { test, expect } from '@playwright/test';

test('searching "rytas" returns "tyras" as an anagram', async ({ page }) => {
  await page.goto('/');

  await page.fill('input[name="id"]', 'rytas');
  await page.click('button[type="Submit"]');

  await expect(page.locator('h2')).toHaveText('Anagrams for "rytas"');
  const results = page.locator('ul li');
  await expect(results).toContainText(['rytas', 'tyras']);
});
