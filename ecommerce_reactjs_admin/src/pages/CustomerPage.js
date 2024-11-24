import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

const CustomerPage = () => {
  const [customers, setCustomers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const navigate = useNavigate();

  const token = localStorage.getItem('token');
  const roles = JSON.parse(localStorage.getItem('roles'));

  useEffect(() => {
    // Kiểm tra quyền truy cập
    if (!token || !roles || !roles.includes('Admin User')) {
      navigate('/login');
    } else {
      // Lấy danh sách users
      axios
        .get('http://localhost:5203/api/Account/basic-users', {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        })
        .then((response) => {
          setCustomers(response.data);
          setLoading(false);
        })
        .catch((error) => {
          setError('There was an error fetching the users!');
          setLoading(false);
        });
    }
  }, [token, roles, navigate]);

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>{error}</div>;
  }

  return (
    <div>
      <h1>Customer Users</h1>
      <table style={{ width: '100%', borderCollapse: 'collapse' }}>
        <thead>
          <tr>
            <th style={{ border: '1px solid #ddd', padding: '8px', textAlign: 'left' }}>Username</th>
            <th style={{ border: '1px solid #ddd', padding: '8px', textAlign: 'left' }}>Email</th>
            <th style={{ border: '1px solid #ddd', padding: '8px', textAlign: 'left' }}>Phone</th>
            <th style={{ border: '1px solid #ddd', padding: '8px', textAlign: 'left' }}>Role</th>
          </tr>
        </thead>
        <tbody>
          {customers.map((customer) => (
            <tr key={customer.userName}>
              <td style={{ border: '1px solid #ddd', padding: '8px' }}>{customer.userName}</td>
              <td style={{ border: '1px solid #ddd', padding: '8px' }}>{customer.email}</td>
              <td style={{ border: '1px solid #ddd', padding: '8px' }}>{customer.phoneNumber}</td>
              <td style={{ border: '1px solid #ddd', padding: '8px' }}>{customer.roleName}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default CustomerPage;
