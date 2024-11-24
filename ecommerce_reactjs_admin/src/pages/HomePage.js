import React, { useEffect, useState } from 'react';
import { Container, Row, Col, Card } from 'react-bootstrap'; // Sử dụng các thành phần của react-bootstrap

const HomePage = () => {
  const [message, setMessage] = useState(
    'Please select the login button at the top right to sign in and start working.'
  );

  useEffect(() => {
    const token = localStorage.getItem('token');
    const role = localStorage.getItem('roles');

    if (token && role && role.includes('Admin User')) {
      setMessage(
        'Please select the menu options displayed such as View Customers, Manage Categories, Manage Products ... to start working.'
      );
    }
  }, []); 

  return (
    <Container>
      <h2 className="text-center my-4">Welcome to Ecommerce Admin</h2>

      <p className="text-center">
        This is the admin dashboard where you can manage various aspects of your ecommerce platform. 
        To access and perform administrative tasks such as managing products, categories, and customers, 
        you must have the appropriate user role (Admin User). If you do not have access, please contact your system administrator.
      </p>

      <Row className="text-center mt-4">
        {/* Card for View Customers */}
        <Col md={4} className="mb-4">
          <Card>
            <Card.Body>
              <div style={{ fontSize: '40px', color: '#007bff' }}>👥</div>
              <Card.Title className="mt-3">Manage Customers</Card.Title>
              <Card.Text>
                View customer accounts and details.
              </Card.Text>
            </Card.Body>
          </Card>
        </Col>

        {/* Card for Manage Categories */}
        <Col md={4} className="mb-4">
          <Card>
            <Card.Body>
              <div style={{ fontSize: '40px', color: '#28a745' }}>📦</div>
              <Card.Title className="mt-3">Manage Categories</Card.Title>
              <Card.Text>
              Manage category inventory and details.
              </Card.Text>
            </Card.Body>
          </Card>
        </Col>

        {/* Card for Manage Products */}
        <Col md={4} className="mb-4">
          <Card>
            <Card.Body>
              <div style={{ fontSize: '40px', color: '#ffc107' }}>⚙️</div>
              <Card.Title className="mt-3">Manage Products</Card.Title>
              <Card.Text>
                Manage product inventory and details.
              </Card.Text>
            </Card.Body>
          </Card>
        </Col>
      </Row>

      <p className="text-center mt-5">
        <strong>{message}</strong>
      </p>
    </Container>
  );
};

export default HomePage;
