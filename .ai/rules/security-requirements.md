# Security Requirements

> **TEMPLATE**: Customize this file for your project. Add your specific security controls, compliance requirements, and threat model.

## Overview

This document defines the security requirements and standards for this project. All code must comply with these requirements to protect against common vulnerabilities and meet regulatory obligations.

## OWASP Top 10 Protection

### 1. Broken Access Control

**Requirements:**
- [ ] Authenticate users before granting access to protected resources
- [ ] Verify authorization for every protected action
- [ ] Deny access by default (whitelist, not blacklist)
- [ ] Validate user permissions on server side, not client side
- [ ] Implement proper session management
- [ ] Log authorization failures for security monitoring

**Example (pseudocode):**
```
function deleteResource(userId, resourceId) {
    // Authenticate: Is the user logged in?
    if (!isAuthenticated(userId)) {
        throw UnauthorizedError("Not authenticated");
    }
    
    // Authorize: Does the user own this resource?
    if (!canUserAccessResource(userId, resourceId)) {
        logSecurityEvent("Unauthorized access attempt", { userId, resourceId });
        throw ForbiddenError("Access denied");
    }
    
    // Perform action
    database.delete(resourceId);
}
```

### 2. Cryptographic Failures

**Requirements:**
- [ ] Never store passwords in plain text (use bcrypt, Argon2, or PBKDF2)
- [ ] Use strong encryption for sensitive data at rest (AES-256)
- [ ] Use TLS 1.2+ for data in transit
- [ ] Generate cryptographically secure random values for tokens
- [ ] Rotate encryption keys periodically
- [ ] Never implement custom cryptography

**Secrets Management:**
- NO hardcoded secrets in code or configuration files
- Use environment variables or secret management services (Azure Key Vault, AWS Secrets Manager)
- Rotate secrets regularly
- Restrict secret access to minimum necessary services

### 3. Injection Attacks

**SQL Injection Prevention:**
```
// WRONG: String concatenation
query = "SELECT * FROM users WHERE id = " + userId;

// RIGHT: Parameterized query
query = "SELECT * FROM users WHERE id = ?";
execute(query, [userId]);
```

**Requirements:**
- [ ] Use parameterized queries or ORMs for all database access
- [ ] Validate and sanitize all user input
- [ ] Use allowlists for input validation when possible
- [ ] Escape special characters in queries if parameterization not possible
- [ ] Limit database permissions (principle of least privilege)

**Command Injection Prevention:**
- [ ] Avoid executing shell commands with user input
- [ ] If unavoidable, use safe APIs with argument arrays (not string concatenation)
- [ ] Validate input against strict allowlist
- [ ] Run commands with minimum necessary privileges

### 4. Insecure Design

**Threat Modeling:**
- Document trust boundaries in system architecture
- Identify assets worth protecting
- Enumerate threats using STRIDE or similar framework
- Design security controls for identified threats

**Security Patterns:**
- Defense in depth (multiple layers of security)
- Fail securely (default deny, fail closed)
- Separation of duties
- Least privilege

### 5. Security Misconfiguration

**Requirements:**
- [ ] Disable debug mode in production
- [ ] Remove default credentials
- [ ] Disable directory listings
- [ ] Configure security headers (CSP, HSTS, X-Frame-Options)
- [ ] Keep dependencies updated
- [ ] Remove unused features and frameworks
- [ ] Configure proper error handling (don't leak stack traces)

**Security Headers (Web Applications):**
```
Content-Security-Policy: default-src 'self'
X-Content-Type-Options: nosniff
X-Frame-Options: DENY
X-XSS-Protection: 1; mode=block
Strict-Transport-Security: max-age=31536000; includeSubDomains
```

### 6. Vulnerable and Outdated Components

**Dependency Management:**
- [ ] Maintain inventory of all dependencies
- [ ] Monitor for known vulnerabilities (Dependabot, Snyk, OWASP Dependency-Check)
- [ ] Update dependencies regularly
- [ ] Remove unused dependencies
- [ ] Verify dependency integrity (checksums, signatures)
- [ ] Review licenses for compliance

**Automated Scanning:**
- Run dependency vulnerability scans in CI/CD pipeline
- Fail builds for high/critical vulnerabilities
- Track and remediate vulnerabilities within SLA

### 7. Identification and Authentication Failures

**Password Requirements:**
- Minimum 12 characters (or 8 with complexity requirements)
- Reject common passwords (use known-bad-password lists)
- Implement rate limiting on login attempts
- Use multi-factor authentication for sensitive operations
- Implement secure password reset flow

**Session Management:**
- [ ] Generate new session IDs after login
- [ ] Invalidate sessions after logout
- [ ] Implement session timeout for inactivity
- [ ] Use secure, httpOnly, sameSite cookies
- [ ] Regenerate session IDs periodically

**Token Security:**
```
// Generate secure random token
token = generateSecureRandom(32); // 256 bits
hash = sha256(token);

// Store hash, return token
database.save({ userId, tokenHash: hash, expires: now() + 1h });
return token;

// Validate token
storedHash = database.getTokenHash(userId);
if (sha256(providedToken) === storedHash && !isExpired()) {
    // Valid
}
```

### 8. Software and Data Integrity Failures

**Requirements:**
- [ ] Verify digital signatures on software updates
- [ ] Use Subresource Integrity (SRI) for CDN resources
- [ ] Implement code signing for releases
- [ ] Review dependencies before adding to project
- [ ] Use lock files for dependency versions
- [ ] Validate deserialized data (no arbitrary code execution)

### 9. Security Logging and Monitoring Failures

**What to Log:**
- Authentication events (success and failure)
- Authorization failures
- Input validation failures
- Security-relevant state changes
- Administrative actions

**What NOT to Log:**
- Passwords or password hashes
- Session tokens or API keys
- Credit card numbers or PII
- Encryption keys

**Log Format:**
```json
{
    "timestamp": "2024-03-20T10:30:00Z",
    "level": "WARN",
    "event": "login_failure",
    "userId": "user@example.com",
    "ip": "192.168.1.100",
    "userAgent": "Mozilla/5.0...",
    "reason": "invalid_password",
    "attemptCount": 3
}
```

**Monitoring:**
- [ ] Alert on suspicious patterns (brute force, privilege escalation)
- [ ] Retain logs for compliance period (90 days minimum)
- [ ] Protect log integrity (append-only, tamper detection)
- [ ] Regular security log review

### 10. Server-Side Request Forgery (SSRF)

**Requirements:**
- [ ] Validate and sanitize all URLs from user input
- [ ] Use allowlist of permitted domains/IPs
- [ ] Block requests to internal IP ranges (10.0.0.0/8, 172.16.0.0/12, 192.168.0.0/16, 127.0.0.0/8)
- [ ] Disable HTTP redirects or validate redirect targets
- [ ] Use network segmentation to isolate internal services

## Input Validation

**Validation Rules:**
1. **Validate on server side** (client-side is convenience, not security)
2. **Use allowlists** when possible (specify what IS allowed)
3. **Reject invalid input** (fail securely, don't try to sanitize)
4. **Validate data type, format, length, range**
5. **Context-specific validation** (email vs URL vs phone number)

**Example:**
```
function validateEmail(email) {
    // Type check
    if (typeof email !== 'string') return false;
    
    // Length check
    if (email.length > 254) return false;
    
    // Format check
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(email)) return false;
    
    // Domain allowlist (if applicable)
    const allowedDomains = ['company.com', 'partner.org'];
    const domain = email.split('@')[1];
    if (!allowedDomains.includes(domain)) return false;
    
    return true;
}
```

## Output Encoding

**Context-Specific Encoding:**

**HTML Context:**
```
// Encode: & < > " ' /
userInput = escapeHtml(userInput);
```

**JavaScript Context:**
```
// Use JSON.stringify for data in script tags
<script>
const data = <?= json_encode($userData) ?>;
</script>
```

**URL Context:**
```
// Encode URL parameters
url = "https://example.com/search?q=" + encodeURIComponent(query);
```

## Authentication & Authorization

**Authentication Methods:**
- [OAuth 2.0 / OIDC / SAML / API Keys] — describe your auth method
- [SSO provider / identity provider details]

**Authorization Model:**
- [RBAC / ABAC / Custom] — describe your authorization model
- Roles: [list roles and their permissions]

**Token Handling:**
- Access token lifetime: [e.g., 15 minutes]
- Refresh token lifetime: [e.g., 7 days]
- Token storage: [httpOnly cookie / Authorization header / secure storage]
- Token revocation: [how tokens are invalidated]

## API Security

**Requirements:**
- [ ] Rate limiting (prevent abuse)
- [ ] API authentication (API keys, OAuth tokens)
- [ ] Input validation for all endpoints
- [ ] Proper HTTP methods (GET for read, POST for create, etc.)
- [ ] CORS configuration (allowlist specific origins)
- [ ] API versioning
- [ ] Comprehensive API documentation

**Rate Limiting Example:**
```
Rate limits:
- Unauthenticated: 100 requests/hour per IP
- Authenticated: 1000 requests/hour per user
- Login endpoint: 5 attempts/15 minutes per IP
```

## Data Protection

### Data Classification

| Classification | Examples | Protection Required |
|----------------|----------|---------------------|
| Public | Marketing materials | None |
| Internal | Business documents | Access control |
| Confidential | Customer data, PII | Encryption + access control |
| Restricted | Credentials, financial data | Encryption + strict access control + audit |

### Personal Data (PII)

**Requirements:**
- [ ] Document what PII is collected and why
- [ ] Obtain user consent for PII collection
- [ ] Encrypt PII at rest and in transit
- [ ] Implement data retention policies
- [ ] Provide data export capability (GDPR/CCPA)
- [ ] Implement data deletion on request
- [ ] Log all PII access

**Examples of PII:**
- Name, email, phone number, address
- Social security number, driver's license
- IP address, device identifiers
- Biometric data, health records

## Compliance Requirements

> **TODO**: Add your specific compliance requirements

### [Regulation Name - e.g., GDPR, HIPAA, PCI-DSS, SOC 2]

**Applicable Requirements:**
- [List specific requirements]
- [Refer to compliance documentation]

## Security Testing

**Required Tests:**
- [ ] Static Application Security Testing (SAST) — automated code analysis
- [ ] Dynamic Application Security Testing (DAST) — runtime testing
- [ ] Dependency vulnerability scanning
- [ ] Penetration testing (annual minimum)
- [ ] Security code review for high-risk changes

**Tools:**
- SAST: [Tool names - e.g., SonarQube, Checkmarx]
- DAST: [Tool names - e.g., OWASP ZAP, Burp Suite]
- Dependency scanning: [Tool names - e.g., Dependabot, Snyk]

## Incident Response

**Security Incident Procedure:**
1. **Detect**: Identify security incident
2. **Contain**: Isolate affected systems
3. **Investigate**: Determine scope and impact
4. **Remediate**: Fix vulnerability, restore systems
5. **Document**: Record incident details and lessons learned
6. **Notify**: Inform stakeholders per legal requirements

**Contact:**
- Security team: [Email/Slack channel]
- Incident reporting: [Process/link]

## Resources

- OWASP Top 10: https://owasp.org/Top10/
- OWASP Cheat Sheets: https://cheatsheetseries.owasp.org/
- Security architecture: [`.ai/memory/architecture.md`](../memory/architecture.md)
- Threat model: [Link to threat model document]

---

**Last updated:** [Date]  
**Review frequency:** Quarterly or after security incidents
