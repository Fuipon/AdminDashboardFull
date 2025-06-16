import { render } from "@testing-library/react";
import Dashboard from "../Pages/Dashboard";

vi.mock("axios", () => {
    return {
        default: {
            create: () => ({
                get: vi.fn((url) => {
                    if (url === "/clients") {
                        return Promise.resolve({ data: [{ id: 1, name: "Client A" }] });
                    }
                    if (url === "/payments?take=5") {
                        return Promise.resolve({ data: [] });
                    }
                    return Promise.reject(new Error("Unknown endpoint"));
                }),
                interceptors: {
                    request: {
                        use: vi.fn(),
                    },
                },
            }),
        },
    };
});

describe("Dashboard", () => {
    it("должен отрендериться без ошибок", async () => {
        render(<Dashboard />);
    });
});
