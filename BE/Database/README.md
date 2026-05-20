# ChatAppDB schema (stable)

## Why this schema
- Uses `UNIQUEIDENTIFIER` for `users.id` to match API model (`Guid`).
- Uses `INT` for `roles.id` and `users.role_id` to keep FK simple and stable.
- Keeps only columns actually used by auth flow plus basic audit fields.

## Apply script
Run `ChatAppDB.schema.sql` in SQL Server Management Studio (or Azure Data Studio) against database `ChatAppDB`.

> Script will **drop and recreate** `users` + `roles` tables.

## Default seed data
- `roles`: `admin` (`id=1`), `user` (`id=2`)

## Expected API behavior after applying
- `POST /api/auth/register` inserts user with `role_id = 2`
- `POST /api/auth/sign-in` validates password and returns JWT + user
