# --- PROVIDER ---
provider "aws" {
  region = "us-east-1" # Certifique-se que é a mesma região onde quer os recursos
}

# --- NETWORK INFRASTRUCTURE (VPC) ---

# 1. Create VPC
resource "aws_vpc" "main_vpc" {
  cidr_block           = "10.0.0.0/16"
  enable_dns_hostnames = true
  enable_dns_support   = true

  tags = { Name = "rds-public-vpc" }
}

# 2. Internet Gateway (This is the entry "doorway" to the internet)
resource "aws_internet_gateway" "igw" {
  vpc_id = aws_vpc.main_vpc.id

  tags = { Name = "rds-igw" }
}

# 3. Subnets (RDS demands 2 subnets in different AZs for the Subnet Group)
resource "aws_subnet" "public_subnet_a" {
  vpc_id                  = aws_vpc.main_vpc.id
  cidr_block              = "10.0.1.0/24"
  availability_zone       = "us-east-1a"
  map_public_ip_on_launch = true

  tags = { Name = "rds-subnet-a" }
}

resource "aws_subnet" "public_subnet_b" {
  vpc_id                  = aws_vpc.main_vpc.id
  cidr_block              = "10.0.2.0/24"
  availability_zone       = "us-east-1b"
  map_public_ip_on_launch = true

  tags = { Name = "rds-subnet-b" }
}

# 4. Tabela de Rotas (Diz à rede para enviar tráfego externo para o IGW)
resource "aws_route_table" "public_rt" {
  vpc_id = aws_vpc.main_vpc.id

  route {
    cidr_block = "0.0.0.0/0"
    gateway_id = aws_internet_gateway.igw.id
  }

  tags = { Name = "rds-public-route-table" }
}

resource "aws_route_table_association" "a" {
  subnet_id      = aws_subnet.public_subnet_a.id
  route_table_id = aws_route_table.public_rt.id
}

resource "aws_route_table_association" "b" {
  subnet_id      = aws_subnet.public_subnet_b.id
  route_table_id = aws_route_table.public_rt.id
}

# --- SECURITY AND DATABASE ---

# 5. RDS Subnet Group (Groups the subnets for the database)
resource "aws_db_subnet_group" "default" {
  name       = "rds-main-subnet-group"
  subnet_ids = [aws_subnet.public_subnet_a.id, aws_subnet.public_subnet_b.id]

  tags = { Name = "My DB subnet group" }
}

# 6. Security Group (The RDS "Firewall")
resource "aws_security_group" "rds_sg" {
  name        = "allow_mysql_access"
  description = "Permitir acesso ao MySQL vindo de qualquer lugar"
  vpc_id      = aws_vpc.main_vpc.id

  ingress {
    from_port   = 3306
    to_port     = 3306
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"] # Open to the world (Watch out: this is just for testing purposes)
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }

  tags = { Name = "rds-security-group" }
}

# 7. RDS MySQL Instance (Configured for the Free Tier)
resource "aws_db_instance" "mysql_db" {
  identifier             = "minha-rds-impecavel"
  allocated_storage      = 20
  db_name                = "meubanco"
  engine                 = "mysql"
  engine_version         = "8.0"
  instance_class         = "db.t3.micro" # Free Tier Compatible
  username               = "admin"
  password               = "MudarSenha123!" # Nota: Em produção, use variáveis
  parameter_group_name   = "default.mysql8.0"
  skip_final_snapshot    = true
  publicly_accessible    = true # Activetes public visibility
  db_subnet_group_name   = aws_db_subnet_group.default.name
  vpc_security_group_ids = [aws_security_group.rds_sg.id]

  tags = { Name = "MySQL-Free-Tier" }
}

# --- OUTPUT ---
# This will show the Database address on the Terraform Cloud after the "apply"
output "db_instance_endpoint" {
  description = "O endpoint da base de dados para conexao"
  value       = aws_db_instance.mysql_db.endpoint
}
