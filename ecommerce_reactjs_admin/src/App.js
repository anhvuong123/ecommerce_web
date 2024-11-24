import React, { useState, useEffect } from 'react';
import { BrowserRouter as Router, Route, Routes, Link } from 'react-router-dom';
import { Container, Navbar, Nav, Button, Row, Col } from 'react-bootstrap';
import CategoryPage from './pages/CategoryPage';
import ProductPage from './pages/ProductPage';
import CustomerPage from './pages/CustomerPage';
import HomePage from './pages/HomePage';
import LoginPage from './pages/LoginPage';
import './App.css';

function App() {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [userRole, setUserRole] = useState(null);

  // Kiểm tra token và roles khi trang được load
  useEffect(() => {
    const token = localStorage.getItem('token');
    const roles = JSON.parse(localStorage.getItem('roles'));

    if (token && roles && roles.includes('Admin User')) {
      setIsLoggedIn(true);
      setUserRole(roles);
    }
  }, []);

  // Hàm xử lý đăng xuất
  const handleLogout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('roles');
    setIsLoggedIn(false);
    setUserRole(null);
  };

  return (
    <Router>
      <div>
        <Navbar bg="dark" variant="dark" expand="lg">
          <Container>
            <Navbar.Brand href="/" className="ecommerce-brand">Ecommerce Admin</Navbar.Brand>
            <Navbar.Toggle aria-controls="basic-navbar-nav" />
            <Navbar.Collapse id="basic-navbar-nav">
              <Nav className="me-auto">
                <Nav.Link as={Link} to="/" className="nav-link">Home</Nav.Link>

                {isLoggedIn && userRole && userRole.includes("Admin User") && (
                  <>
                    <Nav.Link as={Link} to="/customers" className="nav-link">View Customers</Nav.Link>
                    <Nav.Link as={Link} to="/categories" className="nav-link">Manage Categories</Nav.Link>
                    <Nav.Link as={Link} to="/products" className="nav-link">Manage Products</Nav.Link>
                  </>
                )}
              </Nav>
              <Nav.Link as={Link} to="/login">
                {isLoggedIn ? (
                  <Button variant="danger" onClick={handleLogout}>Logout</Button>
                ) : (
                  <Button variant="primary">Login</Button>
                )}
              </Nav.Link>
            </Navbar.Collapse>
          </Container>
        </Navbar>

        <Container className="mt-4">
          <Row>
            <Col>
              <Routes>
                <Route path="/" element={<HomePage />} />
                <Route path="/customers" element={<CustomerPage />} />
                <Route path="/categories" element={<CategoryPage />} />
                <Route path="/products" element={<ProductPage />} />
                <Route path="/login" element={<LoginPage setIsLoggedIn={setIsLoggedIn} setUserRole={setUserRole} />} />
              </Routes>
            </Col>
          </Row>
        </Container>
      </div>
    </Router>
  );
}

export default App;
