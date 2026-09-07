import { createBrowserRouter, RouterProvider } from "react-router-dom";
import Login from "./pages/Login";
import Register from "./pages/Register";
import PrivateRoute from "./components/PrivateRoute";
import Events from "./pages/Events";
import Home from "./pages/Home";
import EventDetails, {
  loader as eventDetailLoader,
} from "./pages/EventsDetails";
import Organization from "./pages/Organization";
import News from "./pages/News";
import OrgDetails, { loader as orgDetailLoader } from "./pages/OrgDetails";
import NewDetails, { loader as newDetailLoader } from "./pages/NewDetails";
import WelcomeRoot from "./components/WelcomeRoot";
import EditEvent from "./pages/EditEvent";
import NewEvent from "./pages/NewEvent";
import EditOrg from "./pages/EditOrg";
import NewOrg from "./pages/NewOrg";
import NewNew from "./pages/NewNew";
import EditNew from "./pages/EditNew";
import { action as logoutAction } from "./pages/Logout";
import Blogs from "./pages/Blogs";
import BlogDetails from "./pages/BlogDetails";
import PageNotFound from "./components/PageNotFound";

const router = createBrowserRouter([
  {
    path: "/",
    errorElement: <PageNotFound />,
    element: <WelcomeRoot />,
    children: [
      { path: "", element: <Home /> },
      {
        path: "events",
        children: [
          { path: "", element: <Events /> },
          {
            path: ":id",
            id: "public-event-details",
            loader: eventDetailLoader,
            element: <EventDetails />,
          },
        ],
      },
      {
        path: "organizations",
        children: [
          { path: "", element: <Organization /> },
          {
            path: ":id",
            id: "public-org-details",
            loader: orgDetailLoader,
            element: <OrgDetails />,
          },
        ],
      },
      {
        path: "news",
        children: [
          { path: "", element: <News /> },
          {
            path: ":id",
            id: "public-new-details",
            loader: newDetailLoader,
            element: <NewDetails />,
          },
        ],
      },
      { path: "blogs", element: <Blogs /> },
    ],
  },
  { path: "/register", element: <Register /> },
  { path: "/login", element: <Login /> },
  {
    path: "/user",
    element: <PrivateRoute />,
    children: [
      { path: "home", element: <Home /> },
      {
        path: "events",
        children: [
          { path: "", element: <Events /> },
          {
            path: ":id",
            id: "private-event-details",
            loader: eventDetailLoader,
            children: [
              { path: "", element: <EventDetails /> },
              { path: "edit", element: <EditEvent /> },
            ],
          },
          { path: "new", element: <NewEvent /> },
        ],
      },
      {
        path: "organizations",
        children: [
          { path: "", element: <Organization /> },
          {
            path: ":id",
            id: "private-org-details",
            loader: orgDetailLoader,
            children: [
              { path: "", element: <OrgDetails /> },
              { path: "edit", element: <EditOrg /> },
            ],
          },
          { path: "new", element: <NewOrg /> },
        ],
      },
      {
        path: "news",
        children: [
          { path: "", element: <News /> },
          {
            path: ":id",
            id: "private-new-details",
            loader: newDetailLoader,
            children: [
              { path: "", element: <NewDetails /> },
              { path: "edit", element: <EditNew /> },
            ],
          },
          { path: "new", element: <NewNew /> },
        ],
      },
      {
        path: "blogs",
        children: [
          { path: "", element: <Blogs /> },
          { path: ":id", element: <BlogDetails /> },
        ],
      },
      { path: "logout", action: logoutAction },
    ],
  },
]);

function App() {
  return <RouterProvider router={router} />;
}

export default App;
