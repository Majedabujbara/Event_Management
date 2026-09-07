import { useRouteLoaderData } from "react-router-dom";
import NewForm from "../components/NewForm";

export default function EditNew() {
  const newData = useRouteLoaderData("private-new-details");
  return <NewForm n={newData} method="PATCH" />;
}
