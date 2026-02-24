# setup terraform/tofu

create a vars file at infra/terraform.tfvars
and fill in the vars from infra/variables.tf

then create a file named infra/aws_creds.ini with this content filled in
```ini
[votemon]
aws_access_key_id = keyid
aws_secret_access_key = secret
```

