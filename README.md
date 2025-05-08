# 🏠 Real Estate Web - Full Stack Application

This is a full-stack real estate management application built as a technical challenge using:
- Backend with **.NET 8 + MongoDB**
- Frontend with **Next.js 14 + Tailwind CSS + React Hook Form + Zod**

---

## 📦 Technologies Used

### Backend (.NET 8, MongoDB)
- ASP.NET Core Web API (.NET 8)
- MongoDB.Driver
- AutoMapper
- Swagger
- Fluent validations (via Zod on frontend)
- Clean Architecture (Domain, Application, Infrastructure, API)
- Physical image upload to `wwwroot/images` + image deletion

### Frontend (Next.js + Tailwind CSS)
- Next.js 14 (App Router)
- Tailwind CSS
- Axios
- React Hook Form + Zod
- React Hot Toast
- Responsive UI + Cards, Filters, Pagination, Modals

---

## ⚒️ Setup Instructions

### 🔧 Requirements
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [MongoDB (local or Atlas)](https://www.mongodb.com/try/download/community)
- [Node.js 18+](https://nodejs.org/)
- [Visual Studio or VS Code](https://code.visualstudio.com/)

---

## ⚙️ Backend

### 🧱 Structure
```bash
src/
├── RealEstate.API             # Web API project
├── RealEstate.Application     # Business logic layer
├── RealEstate.Domain          # Entities & Interfaces
├── RealEstate.Infrastructure  # MongoDB and file storage
├── RealEstate.Test  		   # Unit Test Service
```

### 🔌 Configuration

1. Ensure MongoDB is running on port `27017` locally.
2. In `appsettings.json` of the API project:

```json
"MongoDb": {
  "ConnectionString": "mongodb://localhost:27017",
  "DatabaseName": "RealEstateDb",
  "CollectionName": "Properties"
}
```

3. Install NuGet dependencies:

```bash
cd src/RealEstate.API
dotnet restore
```

4. Run the API:

```bash
dotnet run
```

The API will be available at: `https://localhost:7207/api/properties`  
Swagger documentation: `https://localhost:7207/swagger`

### ✅ Backend Unit Testing

The backend tests live in the `RealEstate.Test` project and cover service logic and repository validations.

#### 🧪 Run backend tests
```bash
cd src/RealEstate.Test
dotnet test
```

> Tests are written using `NUnit` and validate service behavior including:
> - Property creation with image saving
> - Duplicate property checks
> - Image deletion upon property removal

> Ensure dependencies like MongoDB and local filesystem access are mocked for isolated testing.

---

## 🌐 Frontend

### 📁 Folder Structure
```bash
real-estate-app/
├── src/
├── ├__tests__/components/  # Unit Test components
│   ├── app/           		# Pages and routing
│   ├── components/    		# Cards, Modals, Form, Filters
│   ├── services/      		# API calls via Axios
│   ├── types/         		# TypeScript types
```

### 📦 Install Frontend

```bash
cd real-estate-app
npm install
```

### 🚀 Run Frontend

```bash
npm run dev
```

The frontend runs at: `http://localhost:3000`

> Ensure the API is running at `https://localhost:7207`, or adjust `api.ts` in `services`.

---

## 📸 Image Uploads

- Images are saved under `wwwroot/images` in the backend.
- Images are automatically deleted when a property is deleted.

---

## ✅ Implemented Features

### Backend
- Property CRUD
- Filter by name, address, price range
- File system image storage
- Check for duplicates before insert
- XML comments for Swagger
- HTTP error handling (`409 Conflict`, `404 Not Found`, `500 Internal Server Error`)

### Frontend
- Reactive form filters
- Automatic pagination
- Blur modal for details
- Styled validations using Zod
- Toast notifications (success/error)
- Loading spinner and user feedback
- Clean responsive UI

---

### 🧪 Unit Testing (Frontend)

Frontend tests are written using **Jest** and **React Testing Library** and stored in:
```
src/__tests__/components/
```

#### ✅ Covered Components
- `PropertyCard`
- `PropertyModal`
- `FloatingAddButton`
- `PropertyList`
- `PropertyForm`

#### 📦 Install Testing Packages
```bash
npm install --save-dev jest @types/jest ts-jest jest-environment-jsdom @testing-library/react @testing-library/jest-dom @testing-library/user-event
```

#### ⚙️ Run Frontend Tests
```bash
npm test

This will run all `.test.tsx` files under `src/__tests__/**`
```
### 🧰 Tools Used
- [Jest](https://jestjs.io/): JavaScript testing framework
- [React Testing Library](https://testing-library.com/docs/react-testing-library/intro/): For testing React components
- `@testing-library/jest-dom`: Custom matchers for assertions
- `@testing-library/react-hooks`, `@testing-library/user-event`


Ensure your `jest.config.ts` includes:
```ts
export default {
  preset: 'ts-jest',
  testEnvironment: 'jsdom',
  setupFilesAfterEnv: ['<rootDir>/jest.setup.ts'],
  moduleNameMapper: {
    '^@/(.*)$': '<rootDir>/src/$1',
  },
};
```

And create a `jest.setup.ts`:
```ts
import '@testing-library/jest-dom';
```

---

## 🚧 Future Suggestions
- User authentication/login
- Favorite properties
- MongoDB Atlas cloud integration
- Deploy on Vercel, Azure or Railway

---

## 📩 Contact

This project was built as a technical challenge submission.  
For questions or contributions, please contact me(jaiverde996@gmail.com).

---

_Thanks for checking out the project!_ 🚀
