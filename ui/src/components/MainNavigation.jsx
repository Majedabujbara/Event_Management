import { NavLink, Outlet, useNavigate } from "react-router-dom";
import EventOutlinedIcon from "@mui/icons-material/EventOutlined";
import MenuOpenIcon from "@mui/icons-material/MenuOpen";
import IconButton from "@mui/material/IconButton";
import { useState } from "react";
import AccountMenu from "./AccountMenu";
import HomeOutlinedIcon from "@mui/icons-material/HomeOutlined";
import PeopleOutlineOutlinedIcon from "@mui/icons-material/PeopleOutlineOutlined";
import ArticleOutlinedIcon from "@mui/icons-material/ArticleOutlined";
import SummarizeOutlinedIcon from "@mui/icons-material/SummarizeOutlined";
import { Button } from "@mui/material";
import SearchBar from "./SearchBar";
import ForumOutlinedIcon from "@mui/icons-material/ForumOutlined";

export default function Events() {
  const navigate = useNavigate();
  const [toggle, setToggle] = useState(false);
  const isLoggedIn = !!localStorage.getItem("token");

  function handleLogin() {
    navigate("/login");
  }

  const handleNavClick = () => {
    const isSmallScreen = window.innerWidth < 768;
    const isLargeScreen = window.innerWidth >= 768;

    if (isSmallScreen || (isLargeScreen && toggle)) {
      setToggle(false);
    }
  };

  async function handleLogout() {
    try {
      const response = await fetch("https://localhost:7262/api/Account/Logout");

      if (response.ok) {
        localStorage.clear();
        navigate("/");
      } else {
        console.log("Logging out error");
      }
    } catch (error) {
      console.log(error);
    }
  }

  async function handleDelete() {
    const response = await fetch(
      `https://localhost:7262/api/Account/${localStorage.getItem("email")}`,
      {
        method: "DELETE",
        headers: {
          "Content-Type": "application/json",
        },
      }
    );
    if (response.ok) {
      localStorage.clear();
      console.log("Delete good!");
      navigate("/");
    } else {
      console.log("error");
    }
  }

  function handleToggle() {
    setToggle((prevT) => !prevT);
  }

  return (
    <>
      <div className="header-container">
        <div
          className="dashboard-header"
          style={{
            display: "flex",
            alignItems: "center",
            justifyContent: "space-between",
            width: "100%",
            gap: "16px",
          }}
        >
          <div
            className="brand"
            style={{
              display: "flex",
              alignItems: "center",
              minWidth: "fit-content",
            }}
          >
            <IconButton sx={{ width: 40, height: 40 }} onClick={handleToggle}>
              <MenuOpenIcon
                sx={{
                  fontSize: 30,
                  transform: toggle ? undefined : "scaleX(-1)",
                }}
              />
            </IconButton>
            <img src="/Images/BAUClubs.png" alt="" width={"40px"} />
          </div>

          <div
            style={{
              flex: 1,
              display: "flex",
              justifyContent: "center",
              maxWidth: "800px",
              minWidth: "250px",
              position: "relative",
            }}
          >
            <SearchBar />
          </div>

          <div
            style={{
              display: "flex",
              alignItems: "center",
              minWidth: "fit-content",
            }}
          >
            {isLoggedIn ? (
              <AccountMenu
                fLetter={localStorage.getItem("name").charAt(0)}
                email={localStorage.getItem("email")}
                logout={handleLogout}
                delAcc={handleDelete}
              />
            ) : (
              <Button onClick={handleLogin} variant="contained" color="success">
                Login
              </Button>
            )}
          </div>
        </div>
      </div>
      <div className={`acc-nav ${toggle ? "expanded" : "collapsed"}`}>
        <div className="logo-nav">
          <div className="nav-links">
            <NavLink
              to={isLoggedIn ? "home" : "/"}
              onClick={handleNavClick}
              className={({ isActive }) => (isActive ? "active" : undefined)}
              end
            >
              <HomeOutlinedIcon sx={{ fontSize: 28 }} />
              <span className="link-text">Home</span>
            </NavLink>
            <NavLink
              to="events"
              onClick={handleNavClick}
              className={({ isActive }) => (isActive ? "active" : undefined)}
            >
              <EventOutlinedIcon sx={{ fontSize: 28 }} />
              <span className="link-text">Events</span>
            </NavLink>
            <NavLink
              to="organizations"
              onClick={handleNavClick}
              className={({ isActive }) => (isActive ? "active" : undefined)}
            >
              <PeopleOutlineOutlinedIcon sx={{ fontSize: 28 }} />
              <span className="link-text">Organizations</span>
            </NavLink>
            <NavLink
              to="news"
              onClick={handleNavClick}
              className={({ isActive }) => (isActive ? "active" : undefined)}
            >
              <ArticleOutlinedIcon sx={{ fontSize: 28 }} />
              <span className="link-text">News</span>
            </NavLink>
            <NavLink
              to="blogs"
              onClick={handleNavClick}
              className={({ isActive }) => (isActive ? "active" : undefined)}
            >
              <ForumOutlinedIcon sx={{ fontSize: 28 }} />
              <span className="link-text">Blogs</span>
            </NavLink>
          </div>
        </div>
      </div>

      <div className="dashboard-wrapper">
        <Outlet />
      </div>
    </>
  );
}
