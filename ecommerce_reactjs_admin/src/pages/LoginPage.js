import React, { useState } from 'react';
import axios from 'axios';
import { Button, Form } from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';

const LoginPage = ({ setIsLoggedIn, setUserRole }) => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [validationError, setValidationError] = useState('');
  const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();

    setError('');
    setValidationError('');

    if (!username || !password) {
      setValidationError('Username & Password are required!');
      return;
    }

    try {
      const response = await axios.post('http://localhost:5203/api/account/login', {
        username,
        password,
        rememberMe: true,
      });

      if (response.data.token) {
        localStorage.setItem('token', response.data.token);

        const roles = response.data.roles;
        localStorage.setItem('roles', JSON.stringify(roles));

        if (roles && Array.isArray(roles) && roles.includes("Admin User")) {
          setIsLoggedIn(true);
          setUserRole(roles);
          navigate('/'); 
        } else {
          setError('Access denied!');
          localStorage.removeItem('token');
          localStorage.removeItem('roles');
        }
      }
    } catch (err) {
      setError('Invalid credentials, please try again!');
      console.error(err);
    }
  };

  return (
    <div style={{ width: '300px', margin: 'auto', padding: '20px' }}>
      <h2>Login</h2>
      {validationError && <div style={{ color: 'red', marginBottom: '10px' }}>{validationError}</div>}
      {error && <div style={{ color: 'red', marginBottom: '10px' }}>{error}</div>}
      <Form onSubmit={handleLogin}>
        <Form.Group>
          <Form.Label>Username</Form.Label>
          <Form.Control
            type="text"
            placeholder="Enter username"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
          />
        </Form.Group>
        <Form.Group>
          <Form.Label>Password</Form.Label>
          <Form.Control
            type="password"
            placeholder="Enter password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />
        </Form.Group>
        <Button variant="primary" type="submit" style={{ marginTop: '15px' }}>
          Login
        </Button>
      </Form>
    </div>
  );
};

export default LoginPage;
