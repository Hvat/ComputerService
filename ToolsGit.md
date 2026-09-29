# Проверка поведения git_stash после добавления action="drop"

Проведи интеграционное самотестирование `git_stash` после добавления `action="drop"`.

Используй только собственные инструменты CoderAgent. Shell и прямой `git` не используй.

## Сценарий 1 — успешный drop

Создай изменение tracked-файла.
Создай stash через `git_stash push`.
Выполни:

```json
{
  "action": "drop",
  "stashIndex": 0
}
```

Подтверди Approval.

Проверь:

```text
ApprovalRequested
→ ApprovalResolved(Approved)
→ ToolResult.Success
```

Убедись, что именно `stash@{0}` удален.

## Сценарий 2 — Rejected

Создай новый stash и повтори `drop`, но отклони Approval.

Ожидается:

```text
ApprovalRequested
→ ApprovalResolved(Rejected)
→ UserRejected
```

Stash при этом должен сохраниться.

## Сценарий 3 — ошибки

Проверь:

```text
stashIndex = -1
stashIndex = заведомо отсутствующий
```

Ожидается structured error без mutation и без raw stderr.

## Сценарий 4 — точность удаления

Создай минимум два stash.

Удалить один из них и убедиться, что:

- выбранный stash удален;
- остальные stash сохранены;
- repository/worktree не изменены.

## Итог

Выдай таблицу:

| Scenario | Approval | Result | Correct stash removed | Repository unchanged | Status |
| --- | --- | --- | --- | --- | --- |

Статусы: `PASS` / `FAIL`.

Код не исправляй — только проверь фактическое поведение `git_stash`.
