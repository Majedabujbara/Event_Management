import { useRouteLoaderData } from "react-router-dom";
import OrgForm from "../components/OrgForm";

export default function EditOrg() {
  const orgData = useRouteLoaderData("private-org-details");
  return <OrgForm org={orgData} method="PATCH" />;
}
