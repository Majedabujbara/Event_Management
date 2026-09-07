import { Navigate, useSubmit } from "react-router-dom";
import Root from "./Root";
import { useEffect } from "react";
import { getTokenDuration } from "../utility/auth";

export default function PrivateRoute() {
  const token = localStorage.getItem("token");
  const submit = useSubmit();

  useEffect(() => {
    if (!token) {
      return;
    }

    if (token === "EXPIRED") {
      submit(null, { action: "/user/logout", method: "post" });
      return;
    }

    const tokenDuration = getTokenDuration();
    console.log(tokenDuration);

    setTimeout(() => {
      submit(null, { action: "/user/logout", method: "post" });
    }, tokenDuration);
  }, [token, submit]);

  return token ? <Root /> : <Navigate to="/login" />;
}
