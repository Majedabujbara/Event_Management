import { useEffect, useState } from "react";
import { Button, Typography, Divider, Pagination, Box } from "@mui/material";
import { useNavigate } from "react-router-dom";
import { Add } from "@mui/icons-material";
import OrgCard from "../components/OrgCard";
import { getOrgs } from "../utility/apiGetCalls";

export default function Organization() {
  const navigate = useNavigate();
  const [orgs, setOrgs] = useState([]);
  const [currentPage, setCurrentPage] = useState(1);
  const orgsPerPage = 12;

  useEffect(() => {
    async function fetchAndSetOrgs() {
      const resData = await getOrgs();
      if (resData) {
        setOrgs(resData);
      }
    }

    fetchAndSetOrgs();
  }, []);

  const indexOfLastOrg = currentPage * orgsPerPage;
  const indexOfFirstOrg = indexOfLastOrg - orgsPerPage;
  const currentOrgs = orgs.slice(indexOfFirstOrg, indexOfLastOrg);
  const totalPages = Math.ceil(orgs.length / orgsPerPage);

  const handlePageChange = (event, value) => {
    setCurrentPage(value);
  };

  return (
    <div className="organizations-page">
      <div className="organizations-header">
        <Typography variant="h4" component="h1" className="page-title">
          University Organizations
        </Typography>
        {localStorage.getItem("email") === "admin@example.com" && (
          <Button
            variant="contained"
            color="success"
            startIcon={<Add />}
            onClick={() => navigate("new")}
            className="add-org-btn"
          >
            Add Organization
          </Button>
        )}
      </div>

      {orgs.length === 0 ? (
        <Typography className="no-orgs-message">
          No organizations found
        </Typography>
      ) : (
        <>
          <div className="orgs-grid">
            {currentOrgs.map((org) => (
              <OrgCard
                key={org.organizationID}
                name={org.name}
                image={org.logoUrl}
                to={org.organizationID}
                description={org.description}
              />
            ))}
          </div>

          <Box display="flex" justifyContent="center" mt={4}>
            <Pagination
              count={totalPages}
              page={currentPage}
              onChange={handlePageChange}
              variant="outlined"
              sx={{
                "& .MuiPaginationItem-root": {
                  color: "green",
                  borderColor: "green",
                },
                "& .MuiPaginationItem-root.Mui-selected": {
                  backgroundColor: "green",
                  color: "white",
                  borderColor: "green",
                },
                "& .MuiPaginationItem-root.Mui-selected:hover": {
                  backgroundColor: "rgb(177, 255, 177)",
                  color: "black",
                },
                "& .MuiPaginationItem-root:hover": {
                  backgroundColor: "rgb(177, 255, 177)",
                  color: "black",
                },
              }}
            />
          </Box>
        </>
      )}
    </div>
  );
}
