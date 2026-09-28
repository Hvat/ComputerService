# Интеграционное тестирование инструментов Files и Git

Проведи полное интеграционное тестирование реализованных инструментов категорий `Files` и `Git` в текущем `WorkspaceRoot`.

`WorkspaceRoot` — тестовый проект и уже инициализированный Git-репозиторий. Его разрешено свободно изменять: создавать, редактировать, перемещать и удалять файлы и каталоги, создавать commits, branches, stash, выполнять restore и другие доступные локальные Git-операции. Сохранять исходное состояние проекта не требуется.

Цель — проверить фактическое поведение инструментов, взаимодействие и обработку ошибок.

Используй **только инструменты CoderAgent**. Не используй shell, PowerShell, cmd и прямой вызов `git`.

После каждого этапа остановись и выдай промежуточный отчет:

```text
Этап N — <название>

Проверено:
- ...

PASS:
- ...

FAIL:
- ...

PARTIAL / NOT TESTED:
- ...

Обнаруженные проблемы:
- ...

Можно переходить к следующему этапу: Да / Нет
```

При ошибках обязательно проверяй structured error:

```text
ErrorDetails.Code
ErrorDetails.Message
ErrorDetails.RetryHint
```

Ошибки не должны раскрывать stack trace, exception details, raw stderr, sensitive data или лишние внутренние абсолютные пути.

## Этап 1. Workspace и базовое состояние

Проверь:

- `list_files`
- `find_files`
- `get_path_info`
- `git_status`
- `git_branch`
- `git_remote`

Зафиксируй:

- структуру WorkspaceRoot;
- текущую ветку;
- состояние Git;
- tracked/untracked/staged изменения;
- configured remotes.

Не запрашивай Git login/password/PAT/SSH credentials. Локальные Git-операции не требуют аутентификации. `git_remote` тестируй только как чтение локальной конфигурации remote.

После этапа выдай отчет.

## Этап 2. Files — discovery и чтение

Проверь:

- `list_files`
- `find_files`
- `get_path_info`
- `read_file`
- `search_text`

Создай необходимые тестовые файлы и каталоги.

Проверь:

- listing корня и вложенных каталогов;
- `find_files` по точному имени;
- `find_files` по маске;
- поиск `*.cs`, `*.json` и других существующих расширений;
- поиск отсутствующего имени;
- metadata файла;
- metadata каталога;
- чтение полного файла;
- чтение диапазона строк;
- поиск существующего текста;
- поиск отсутствующего текста;
- поиск текста во вложенном каталоге;
- nonexistent path;
- абсолютный path;
- `..`;
- `.git/...`;
- sensitive path;
- directory вместо файла;
- некорректный line range.

После этапа выдай отчет.

## Этап 3. Files — создание и patch

Проверь:

- `create_directory`
- `apply_patch`
- `read_file`
- `get_path_info`

Проверь сценарии:

1. Создание нового каталога.
2. Создание вложенного каталога.
3. Создание нового текстового файла.
4. Изменение существующего файла через точный `oldText`.
5. Несколько последовательных изменений.
6. Проверка результата через `read_file`.

Negative cases:

- existing directory;
- отсутствующий parent;
- отсутствующий `oldText`;
- неоднозначный `oldText`;
- пустой `oldText`;
- неверный тип аргумента;
- absolute path;
- traversal через `..`;
- `.git`;
- sensitive path.

После этапа выдай отчет.

## Этап 4. Files — copy и move

Проверь:

- `copy_file`
- `move_path`
- `read_file`
- `get_path_info`
- `list_files`

Проверь:

1. Копирование файла.
2. Проверку точности содержимого копии.
3. Переименование файла.
4. Перемещение файла.
5. Перемещение каталога.
6. Проверку структуры после операции.

Negative cases:

- отсутствующий source;
- destination уже существует;
- source == destination;
- отсутствующий destination parent;
- directory into descendant;
- sensitive file;
- `.git`;
- path outside WorkspaceRoot.

Если операция требует approval — отдельно проверь `Approved` и `Rejected`.

После этапа выдай отчет.

## Этап 5. Files — delete

Проверь:

- `delete_file`
- `delete_directory`
- `get_path_info`
- `list_files`

Проверь:

- удаление существующего файла;
- повторное удаление отсутствующего файла;
- directory через `delete_file`;
- удаление пустого каталога;
- удаление непустого каталога;
- WorkspaceRoot;
- `.git`;
- sensitive path;
- absolute path;
- traversal.

Для операций с approval проверь:

- Approved;
- Rejected;
- отсутствие mutation после Rejected.

После этапа выдай отчет.

## Этап 6. Git — read-only базовые инструменты

Проверь:

- `git_status`
- `git_history`
- `git_branch`
- `git_tracked_files`
- `git_remote`

Создай при необходимости несколько commits и изменений для подготовки дальнейших этапов.

Проверь:

- clean/dirty status;
- staged/unstaged/untracked;
- текущую branch;
- несколько commits истории;
- tracked files;
- configured remotes;
- отсутствие remote network access;
- отсутствие требования credentials;
- structured errors.

После этапа выдай отчет.

## Этап 7. Git — diff, show, blame

Проверь:

- `git_diff`
- `git_show`
- `git_blame`

Подготовь историю изменений нескольких текстовых файлов.

`git_diff`:

- stat;
- full;
- working-tree changes;
- staged changes;
- конкретный path;
- отсутствие изменений.

`git_show`:

- HEAD;
- конкретный commit SHA;
- metadata;
- patch;
- historical file;
- invalid ref;
- отсутствующий historical path.

`git_blame`:

- полный файл;
- одна строка;
- диапазон строк;
- invalid range;
- untracked file;
- отсутствующий path.

После этапа выдай отчет.

## Этап 8. Git — ignore

Проверь:

- `git_check_ignore`

Создай `.gitignore` и набор ignored/non-ignored файлов.

Проверь:

- ignored file;
- non-ignored file;
- nested rule;
- negated rule;
- directory rule;
- exact rule;
- unusual filename, если поддерживается;
- missing path;
- invalid path;
- `.git`;
- path outside WorkspaceRoot.

Проверь корректность информации о matched ignore rule.

После этапа выдай отчет.

## Этап 9. Git — staging

Проверь:

- `git_stage`
- `git_status`
- `git_diff`

Подготовь одновременно:

- новый untracked file;
- modified tracked file;
- deleted tracked file.

Stage их по exact paths.

Проверь:

- новый файл;
- modification;
- deletion;
- несколько paths;
- что неуказанные изменения не stage-ятся;
- результат через `git_status`;
- staged diff.

Negative cases:

- directory;
- nonexistent path;
- ignored untracked file;
- sensitive path;
- `.git`;
- wildcard/pathspec;
- traversal;
- absolute path.

После этапа выдай отчет.

## Этап 10. Git — commit

Проверь:

- `git_commit`
- `git_status`
- `git_history`
- `git_show`

Сценарий:

1. Создай staged изменения.
2. Оставь отдельное unstaged изменение.
3. Выполни commit.
4. Проверь commit через history/show.
5. Убедись, что unstaged изменение в commit не попало.

Проверь также:

- commit нескольких staged файлов;
- deletion commit;
- commit без staged changes;
- empty/invalid message;
- слишком длинный message;
- отсутствие auto-stage.

После этапа выдай отчет.

## Этап 11. Git — branches

Проверь:

- `git_branch`
- `git_switch_branch`

Проверь:

1. Создание новой branch с `create=true`.
2. Проверку через `git_branch`.
3. Commit в новой branch.
4. Переключение на существующую branch.
5. Возврат обратно.
6. Проверку различий между branches.

Negative cases:

- missing branch;
- `create=true` для existing branch;
- invalid branch name;
- dirty tracked worktree;
- remote branch guessing;
- попытка небезопасного переключения.

После этапа выдай отчет.

## Этап 12. Git — restore

Проверь:

- `git_restore`
- `git_status`
- `git_diff`
- `read_file`

Проверь restore:

- из `index`;
- из `HEAD`;
- из конкретного `ref`;
- одного файла;
- нескольких exact paths;
- удаленного tracked file.

Для каждого соответствующего сценария проверь approval.

Отдельно проверь:

- Approved;
- Rejected;
- отсутствие mutation после Rejected;
- изменение target/source во время ожидания approval, если возможно;
- missing source path;
- invalid ref;
- sensitive path;
- index остается неизменным при working-tree restore.

После этапа выдай отчет.

## Этап 13. Git — stash push

Проверь:

- `git_stash`
- `git_status`
- `git_diff`

Подготовь одновременно:

- staged modification;
- unstaged modification;
- untracked file;
- ignored file.

Проверь:

```json
{
  "action": "push"
}
```

и:

```json
{
  "action": "push",
  "includeUntracked": true
}
```

Проверь:

- approval Approved;
- approval Rejected;
- tracked changes сохраняются;
- staged state сохраняется внутри stash;
- untracked при `includeUntracked=true`;
- untracked остается при `false`;
- ignored file не сохраняется через `includeUntracked`;
- worktree/index после push;
- push без изменений;
- sensitive path.

После этапа выдай отчет.

## Этап 14. Git — stash apply

Проверь:

```json
{
  "action": "apply",
  "stashIndex": 0
}
```

и:

```json
{
  "action": "apply",
  "stashIndex": 0,
  "restoreIndex": true
}
```

Проверь:

- approval Approved;
- approval Rejected;
- восстановление tracked content;
- восстановление untracked content;
- staged state при `restoreIndex=true`;
- отсутствие staged restoration при `restoreIndex=false`;
- stash после apply остается;
- missing stash index;
- dirty worktree;
- conflicting untracked path;
- sensitive path.

После этапа выдай отчет.

## Этап 15. GitInitTool

Проверь `git_init` на текущем уже инициализированном repository.

Проверь:

- повторный вызов на существующем Git repository;
- отсутствие повреждения `.git`;
- корректный structured result/error;
- сохранение доступности `git_status`, `git_history`, `git_branch` после вызова.

Happy-path инициализации нового WorkspaceRoot в рамках текущего теста не требуется. Если инструмент принципиально требует неинициализированный WorkspaceRoot, пометь этот сценарий как `NOT TESTED`, а не как defect.

После этапа выдай отчет.

## Этап 16. Cross-tool security и error handling

Проверь применимые инструменты на:

- malformed JSON arguments;
- missing required arguments;
- unknown argument;
- wrong JSON type;
- absolute paths;
- traversal `..`;
- `.git`;
- missing files;
- wrong object type;
- sensitive paths;
- invalid enums;
- invalid refs.

Для каждого controlled failure проверяй:

```text
Success == false
ErrorDetails.Code != empty
ErrorDetails.Message != empty
ErrorDetails.RetryHint != empty
```

Не должно быть утечки:

```text
stack trace
raw exception
raw stderr
credentials
sensitive content
непредусмотренных внутренних путей
```

После этапа выдай отчет.

## Этап 17. Полный Files workflow

Выполни связный сценарий:

```text
create_directory
    ↓
apply_patch
    ↓
read_file
    ↓
copy_file
    ↓
move_path
    ↓
find_files
    ↓
search_text
    ↓
get_path_info
    ↓
delete_file
    ↓
delete_directory
```

Проверь итоговое состояние через `list_files`.

После этапа выдай отчет.

## Этап 18. Полный Git workflow

Выполни:

```text
git_status
    ↓
apply_patch
    ↓
git_diff
    ↓
git_stage
    ↓
git_status
    ↓
git_commit
    ↓
git_history
    ↓
git_show
    ↓
git_blame
```

Затем:

```text
git_switch_branch(create=true)
    ↓
apply_patch
    ↓
git_stage
    ↓
git_commit
    ↓
git_switch_branch
    ↓
git_diff / git_show
```

Затем:

```text
modify files
    ↓
git_stash(push)
    ↓
approval
    ↓
git_status
    ↓
git_stash(apply)
    ↓
approval
    ↓
git_status
```

И:

```text
modify tracked file
    ↓
git_diff
    ↓
git_restore(HEAD)
    ↓
approval
    ↓
read_file
    ↓
git_diff
```

После этапа выдай отчет.

## Финальный отчет

Сформируй итоговую таблицу:

| Категория | Инструмент | Статус | Happy path | Negative cases | Approval | Structured errors | Проблемы |
| --- | --- | --- | --- | --- | --- | --- | --- |

Статусы:

```text
PASS
FAIL
PARTIAL
NOT TESTED
```

Убедись, что в таблице присутствуют все реализованные инструменты:

```text
Files:
list_files
search_text
find_files
read_file
apply_patch
create_directory
copy_file
move_path
delete_file
delete_directory
get_path_info

Git:
git_init
git_status
git_history
git_diff
git_blame
git_show
git_branch
git_tracked_files
git_check_ignore
git_remote
git_stage
git_commit
git_switch_branch
git_restore
git_stash
```

Для каждой найденной проблемы укажи:

```text
Tool:
Scenario:
Expected:
Actual:
Error code:
Severity: Critical / High / Medium / Low
Reproducible: Yes / No
```

В конце отдельно перечисли:

1. Critical defects.
2. Security issues.
3. WorkspaceRoot violations.
4. Approval issues.
5. Structured error handling issues.
6. Cross-tool integration issues.
7. Не протестированные сценарии и причину.
8. Инструменты, требующие исправления.

Не исправляй обнаруженные проблемы. После финального отчета остановись и дождись отдельного задания.
